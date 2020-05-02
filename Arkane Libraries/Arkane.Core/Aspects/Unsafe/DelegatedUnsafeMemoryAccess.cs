#region header

// Arkane.Core - DelegatedUnsafeMemoryAccess.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:25 AM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Runtime.InteropServices ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Unsafe
{
    /// <summary>
    ///     Hosts information about memory segments that can be accessed without triggering errors. Do not refer to this
    ///     class directly in your code. Use <c>PostSharp.Community.UnsafeMemoryChecker.UnsafeMemoryAccess</c> instead.
    /// </summary>
    [CLSCompliant(false)]
    [UsedImplicitly]
    public static unsafe class DelegatedUnsafeMemoryAccess
    {
        #region Nested type: Segment

        private struct Segment : IComparable <Segment>
        {
            public readonly IntPtr AllocAddress ;
            public readonly int    AllocSize ;
            public readonly IntPtr LiveAddress ;
            public readonly int    LiveSize ;
            public readonly string? AllocMemberName ;
            public readonly string? AllocSourceFilePath ;
            public readonly int    AllocSourceLineNumber ;
            public readonly string? DeallocMemberName ;
            public readonly string? DeallocSourceFilePath ;
            public readonly int    DeallocSourceLineNumber ;
            public readonly bool   Deallocated ;

            public Segment (IntPtr allocAddress, int allocSize)
            {
                this.AllocAddress            = allocAddress ;
                this.AllocSize               = allocSize ;
                this.LiveAddress             = allocAddress ;
                this.LiveSize                = allocSize ;
                this.AllocMemberName         = null ;
                this.AllocSourceFilePath     = null ;
                this.AllocSourceLineNumber   = 0 ;
                this.DeallocMemberName       = null ;
                this.DeallocSourceFilePath   = null ;
                this.DeallocSourceLineNumber = 0 ;
                this.Deallocated             = false ;
            }

            public Segment (IntPtr allocAddress,
                            int    allocSize,
                            IntPtr liveAddress,
                            int    liveSize,
                            string memberName,
                            string sourceFilePath,
                            int    sourceLineNumber)
            {
                this.AllocAddress            = allocAddress ;
                this.AllocSize               = allocSize ;
                this.LiveAddress             = liveAddress ;
                this.LiveSize                = liveSize ;
                this.AllocMemberName         = memberName ;
                this.AllocSourceFilePath     = sourceFilePath ;
                this.AllocSourceLineNumber   = sourceLineNumber ;
                this.DeallocMemberName       = null ;
                this.DeallocSourceFilePath   = null ;
                this.DeallocSourceLineNumber = 0 ;
                this.Deallocated             = false ;
            }

            public Segment (Segment allocatedSegment,
                            string  deallocMemberName,
                            string  deallocSourceFilePath,
                            int     deallocSourceLineNumber)
            {
                this.AllocAddress            = allocatedSegment.AllocAddress ;
                this.AllocSize               = allocatedSegment.AllocSize ;
                this.LiveAddress             = allocatedSegment.LiveAddress ;
                this.LiveSize                = allocatedSegment.LiveSize ;
                this.AllocMemberName         = allocatedSegment.AllocMemberName ;
                this.AllocSourceFilePath     = allocatedSegment.AllocSourceFilePath ;
                this.AllocSourceLineNumber   = allocatedSegment.AllocSourceLineNumber ;
                this.DeallocMemberName       = deallocMemberName ;
                this.DeallocSourceFilePath   = deallocSourceFilePath ;
                this.DeallocSourceLineNumber = deallocSourceLineNumber ;
                this.Deallocated             = true ;
            }

            public Segment Dealloc (string memberName, string sourceFilePath, int sourceLineNumber) =>
                new Segment (this, memberName, sourceFilePath, sourceLineNumber) ;

            public int CompareTo (Segment other) => this.AllocAddress.ToInt64 ().CompareTo (other.AllocAddress.ToInt64 ()) ;

            public override string ToString ()
            {
                if (!this.Deallocated)
                {
                    if ((this.AllocAddress != this.LiveAddress) || (this.AllocSize != this.LiveSize))
                        return string.Format ("({0:x}){1:x}-{2:x}({3:x})[{4};{5}:{6}]",
                                              (ulong) this.AllocAddress,
                                              (ulong) this.LiveAddress,
                                              (ulong) ((byte*) this.LiveAddress  + this.LiveSize),
                                              (ulong) ((byte*) this.AllocAddress + this.AllocSize),
                                              this.AllocMemberName,
                                              this.AllocSourceFilePath,
                                              this.AllocSourceLineNumber) ;

                    return string.Format ("{0:x}-{1:x}[{2};{3}:{4}]",
                                          (ulong) this.LiveAddress,
                                          (ulong) ((byte*) this.LiveAddress + this.LiveSize),
                                          this.AllocMemberName,
                                          this.AllocSourceFilePath,
                                          this.AllocSourceLineNumber) ;
                }

                if ((this.AllocAddress != this.LiveAddress) || (this.AllocSize != this.LiveSize))
                    return string.Format ("({0:x}){1:x}-{2:x}({3:x})[{4};{5}:{6}]->[{7};{8}:{9}]",
                                          (ulong) this.LiveAddress,
                                          (ulong) ((byte*) this.LiveAddress  + this.LiveSize),
                                          (ulong) ((byte*) this.LiveAddress  + this.LiveSize),
                                          (ulong) ((byte*) this.AllocAddress + this.AllocSize),
                                          this.AllocMemberName,
                                          this.AllocSourceFilePath,
                                          this.AllocSourceLineNumber,
                                          this.DeallocMemberName,
                                          this.DeallocSourceFilePath,
                                          this.DeallocSourceLineNumber) ;

                return string.Format ("{0:x}-{1:x}[{2};{3}:{4}]->[{5};{6}:{7}]",
                                      (ulong) this.LiveAddress,
                                      (ulong) ((byte*) this.LiveAddress + this.LiveSize),
                                      this.AllocMemberName,
                                      this.AllocSourceFilePath,
                                      this.AllocSourceLineNumber,
                                      this.DeallocMemberName,
                                      this.DeallocSourceFilePath,
                                      this.DeallocSourceLineNumber) ;
            }
        }

        #endregion

        private const           int            memoryPadding     = 64 ;
        private static readonly List <Segment> safeSegments      = new List <Segment> (65536) ;
        private static readonly List <Segment> tombstones        = new List <Segment> (65536) ;
        private static readonly byte[]         paddingMagicBytes = {0xDE, 0xAD, 0xBE, 0xEF} ;

        private static readonly GCHandle paddingMagicHandle =
            GCHandle.Alloc (DelegatedUnsafeMemoryAccess.paddingMagicBytes, GCHandleType.Pinned) ;

        private static readonly byte* paddingMagicPtr =
            (byte*) DelegatedUnsafeMemoryAccess.paddingMagicHandle.AddrOfPinnedObject () ;

        public static int GetAllocSize (int size) => size + 2 * DelegatedUnsafeMemoryAccess.memoryPadding ;

        public static int GetLiveSize (int size) => size - 2 * DelegatedUnsafeMemoryAccess.memoryPadding ;

        public static int GetLiveOffset () => DelegatedUnsafeMemoryAccess.memoryPadding ;

        public static IntPtr GetLiveAddress (IntPtr allocAddress) => allocAddress + DelegatedUnsafeMemoryAccess.memoryPadding ;

        public static void AddSafeSegment (void*  allocAddress,
                                           int    allocSize,
                                           string memberName,
                                           string sourceFilePath,
                                           int    sourceLineNumber)
        {
            DelegatedUnsafeMemoryAccess.AddSafeSegment ((IntPtr) allocAddress,
                                                        allocSize,
                                                        memberName,
                                                        sourceFilePath,
                                                        sourceLineNumber) ;
        }


        private static void AddSafeSegment (IntPtr allocAddress,
                                            int    allocSize,
                                            string memberName,
                                            string sourceFilePath,
                                            int    sourceLineNumber)
        {
            DelegatedUnsafeMemoryAccess.AddSafeSegment (allocAddress,
                                                        allocSize,
                                                        allocAddress,
                                                        allocSize,
                                                        memberName,
                                                        sourceFilePath,
                                                        sourceLineNumber) ;
        }


        public static void AddSafeSegment (IntPtr allocAddress,
                                           int    allocSize,
                                           IntPtr liveAddress,
                                           int    liveSize,
                                           string memberName,
                                           string sourceFilePath,
                                           int    sourceLineNumber)
        {
            lock (DelegatedUnsafeMemoryAccess.safeSegments)
            {
                var segment = new Segment (allocAddress,
                                           allocSize,
                                           liveAddress,
                                           liveSize,
                                           memberName,
                                           sourceFilePath,
                                           sourceLineNumber) ;
                DelegatedUnsafeMemoryAccess.PadSegment (segment) ;
                int index = DelegatedUnsafeMemoryAccess.GetNearestLesserSegment (DelegatedUnsafeMemoryAccess.safeSegments,
                                                                                 liveAddress) ;

                // Check internal overlap.

                DelegatedUnsafeMemoryAccess.CheckOverlapWithExisting (DelegatedUnsafeMemoryAccess.safeSegments, segment) ;

                if (index == DelegatedUnsafeMemoryAccess.safeSegments.Count - 1)
                    DelegatedUnsafeMemoryAccess.safeSegments.Add (segment) ;
                else
                    DelegatedUnsafeMemoryAccess.safeSegments.Insert (index + 1, segment) ;
            }
        }


        public static void RemoveSafeSegment (IntPtr address,
                                              string memberName,
                                              string sourceFilePath,
                                              int    sourceLineNumber)
        {
            lock (DelegatedUnsafeMemoryAccess.safeSegments)
            {
                int index = DelegatedUnsafeMemoryAccess.safeSegments.BinarySearch (new Segment (address, 0)) ;

                if (index < 0)
                    throw new InvalidOperationException ("Could not find the segment.") ;

                if (DelegatedUnsafeMemoryAccess.safeSegments[index].AllocAddress != address)
                    throw new InvalidOperationException ("Matched segment alloc address invalid.") ;

                Segment segment = DelegatedUnsafeMemoryAccess.safeSegments[index] ;
                DelegatedUnsafeMemoryAccess.safeSegments.RemoveAt (index) ;
                DelegatedUnsafeMemoryAccess.CheckSegmentPadding (segment) ;
                segment = segment.Dealloc (memberName, sourceFilePath, sourceLineNumber) ;

                int tombIndex =
                    DelegatedUnsafeMemoryAccess.GetNearestLesserSegment (DelegatedUnsafeMemoryAccess.tombstones, address) ;

                if (tombIndex >= DelegatedUnsafeMemoryAccess.tombstones.Count - 1)
                    DelegatedUnsafeMemoryAccess.tombstones.Add (segment) ;
                else
                    DelegatedUnsafeMemoryAccess.tombstones.Insert (tombIndex + 1, segment) ;
            }
        }


        public static void CheckAddress (void* address, int size)
        {
            lock (DelegatedUnsafeMemoryAccess.safeSegments)
            {
                int index = DelegatedUnsafeMemoryAccess.GetNearestLesserSegment (DelegatedUnsafeMemoryAccess.safeSegments,
                                                                                 (IntPtr) address) ;

                if ((index < 0) ||
                    ((ulong) address + (uint) size >
                     (ulong) DelegatedUnsafeMemoryAccess.safeSegments[index].LiveAddress +
                     (uint) DelegatedUnsafeMemoryAccess.safeSegments[index].LiveSize))
                {
                    int tombIndex =
                        DelegatedUnsafeMemoryAccess.GetNearestLesserSegment (DelegatedUnsafeMemoryAccess.tombstones,
                                                                             (IntPtr) address) ;

                    throw new AccessViolationException (
                                                        $"Block {(ulong) address:x}-{(ulong) address + (uint) size:x} is not in a safe segment. Closest live segment: {(index < 0 ? null : (Segment?) DelegatedUnsafeMemoryAccess.safeSegments[index])}. Closest tombstones segment: {(tombIndex < 0 ? null : (Segment?) DelegatedUnsafeMemoryAccess.tombstones[tombIndex])}") ;
                }
            }
        }

        private static void CheckOverlapWithExisting (List <Segment> segments, Segment newSegment)
        {
            int prevSegment = DelegatedUnsafeMemoryAccess.GetNearestLesserSegment (segments, newSegment.AllocAddress) ;
            int nextSegment =
                DelegatedUnsafeMemoryAccess.GetNearestHigherSegment (segments, newSegment.AllocAddress + newSegment.AllocSize) ;

            for (int i = prevSegment; i < nextSegment; i++)
            {
                if ((i < 0) || (i >= segments.Count))
                    continue ;

                if (DelegatedUnsafeMemoryAccess.Overlaps (segments[i], newSegment))
                    throw new AccessViolationException (
                                                        $"New segment {newSegment} overlaps with live segment {segments[i]}") ;
            }
        }

        private static bool Overlaps (Segment a, Segment b)
        {
            if (((byte*) a.AllocAddress < (byte*) b.AllocAddress + b.AllocSize) &&
                ((byte*) b.AllocAddress < (byte*) a.AllocAddress + a.AllocSize))
                return true ;

            return false ;
        }

        private static void PadSegment (Segment segment)
        {
            for (var ptr = (byte*) segment.AllocAddress; ptr < (byte*) segment.LiveAddress; ptr++)
                *ptr = DelegatedUnsafeMemoryAccess.paddingMagicPtr[(ulong) ptr % 4] ;

            for (byte* ptr = (byte*) segment.LiveAddress + segment.LiveSize;
                 ptr < (byte*) segment.AllocAddress + segment.AllocSize;
                 ptr++)
                *ptr = DelegatedUnsafeMemoryAccess.paddingMagicPtr[(ulong) ptr % 4] ;
        }

        private static void CheckSegmentPadding (Segment segment)
        {
            for (var ptr = (byte*) segment.AllocAddress; ptr < (byte*) segment.LiveAddress; ptr++)
            {
                if (*ptr != DelegatedUnsafeMemoryAccess.paddingMagicPtr[(ulong) ptr % 4])
                    throw new Exception ($"Diagnostic padding of block {segment} was changed.") ;
            }

            for (byte* ptr = (byte*) segment.LiveAddress + segment.LiveSize;
                 ptr < (byte*) segment.AllocAddress + segment.AllocSize;
                 ptr++)
            {
                if (*ptr != DelegatedUnsafeMemoryAccess.paddingMagicPtr[(ulong) ptr % 4])
                    throw new Exception ($"Diagnostic padding of block {segment} was changed.") ;
            }
        }

        private static int GetNearestLesserSegment (List <Segment> segments, IntPtr address)
        {
            int index = segments.BinarySearch (new Segment (address, 0)) ;

            if (index < 0)
                return ~index - 1 ;

            return index ;
        }

        private static int GetNearestHigherSegment (List <Segment> segments, IntPtr address)
        {
            int index = segments.BinarySearch (new Segment (address, 0)) ;

            if (index < 0)
                return ~index ;

            return index ;
        }
    }
}

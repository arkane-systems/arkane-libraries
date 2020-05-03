using System;
using System.Collections.Generic;
using System.Globalization ;
using System.Text;

using JetBrains.Annotations ;

using PostSharp.Patterns.Contracts ;

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     Represents a storage-metric size; i.e., one in bytes.
    /// </summary>
    [PublicAPI]
    public struct ByteSize : IComparable <ByteSize>, IEquatable <ByteSize>, IFormattable
    {
        #region Public constants

        /// <summary>
        ///     Number of bits in a byte.
        /// </summary>
        /// <remarks>
        ///     This is technically non-portable, but I can't think of any .NET framework implementations
        ///     these days that run on non-8-bit-byte systems. Hell, I can't even think of any non-8-bit-byte
        ///     systems off the top of my head, even though I know they're out there.
        /// </remarks>
        public const long BitsInByte = 8 ;

        /// <summary>
        ///     Represents the smallest possible value of <see cref="ByteSize" />. This field is constant and read-only.
        /// </summary>
        /// <remarks>
        ///     We cap the minimum value of ByteSize at zero; negative amounts of storage are nonsensical.
        /// </remarks>
        public static readonly ByteSize MinValue = new ByteSize (0) ;

        /// <summary>
        ///     Represents the largest possible value of ByteSize. This field is constant and read-only.
        /// </summary>
        /// <remarks>
        ///     The potential number of bits is the limiting value, to prevent long overflows;
        ///     <see cref="Decimal" /> can go much higher, but there's plenty of room for practical
        ///     purposes for now.
        /// </remarks>
        public static readonly ByteSize MaxValue = ByteSize.FromBits (long.MaxValue) ;

        #endregion

        #region Private constants

        private const decimal BytesInKilobyte = 1000m ;
        private const decimal BytesInMegabyte = 1000m * 1000m ;
        private const decimal BytesInGigabyte = 1000m * 1000m * 1000m ;
        private const decimal BytesInTerabyte = 1000m * 1000m * 1000m * 1000m ;
        private const decimal BytesInPetabyte = 1000m * 1000m * 1000m * 1000m * 1000m ;

        private const decimal BytesInKibibyte = 1024m ;
        private const decimal BytesInMebibyte = 1024m * 1024m ;
        private const decimal BytesInGibibyte = 1024m * 1024m * 1024m ;
        private const decimal BytesInTebibyte = 1024m * 1024m * 1024m * 1024m ;
        private const decimal BytesInPebibyte = 1024m * 1024m * 1024m * 1024m * 1024m ;

        private const string BitSymbol = "b" ;
        private const string ByteSymbol = "B" ;
        private const string KilobyteSymbol = "KB" ;
        private const string MegabyteSymbol = "MB" ;
        private const string GigabyteSymbol = "GB" ;
        private const string TerabyteSymbol = "TB" ;
        private const string PetabyteSymbol = "PB" ;
        private const string KibibyteSymbol = "KiB" ;
        private const string MebibyteSymbol = "MiB" ;
        private const string GibibyteSymbol = "GiB" ;
        private const string TebibyteSymbol = "TiB" ;
        private const string PebibyteSymbol = "PiB" ;

        #endregion

        #region Constructors

        /// <summary>
        ///     Create a new <see cref="ByteSize" /> representing the specified number of bytes.
        /// </summary>
        /// <param name="bytes">The number of bytes to represent. May not exceed <see cref="ByteSize.MaxValue" />.</param>
        public ByteSize ([Range (0, long.MaxValue / 8 + 1)] decimal bytes)
            : this ()
        {
            // Check for ceiling, since bits are indivisible.
            var bits = (long) System.Math.Ceiling (bytes * ByteSize.BitsInByte) ;

            this.Bytes = (decimal) bits / ByteSize.BitsInByte ;
        }

        /// <summary>
        ///     Create a new <see cref="ByteSize" /> representing the specified number of bytes.
        /// </summary>
        /// <param name="bytes">The number of bytes to represent. May not exceed <see cref="ByteSize.MaxValue" />.</param>
        public ByteSize (int bytes)
            : this ((decimal) bytes)
        { }

        /// <summary>
        ///     Create a new <see cref="ByteSize" /> representing the specified number of bytes.
        /// </summary>
        /// <param name="bytes">The number of bytes to represent. May not exceed <see cref="ByteSize.MaxValue" />.</param>
        public ByteSize (long bytes)
            : this ((decimal) bytes)
        { }

        #endregion

        #region Static construction methods

        /// <summary>
        ///     Return a <see cref="ByteSize" /> based on a specified number of bits, using the default byte size.
        /// </summary>
        /// <param name="bits">The number of bits.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="bits" /> bits.</returns>
        public static ByteSize FromBits (long bits) => new ByteSize (bits / (decimal) ByteSize.BitsInByte) ;

        /// <summary>
        ///     Return a <see cref="ByteSize" /> based on a specified number of bytes.
        /// </summary>
        /// <param name="bytes">The number of bytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="bytes" /> bytes.</returns>
        public static ByteSize FromBytes (decimal bytes) => new ByteSize (bytes) ;

        /// <summary>
        ///     <para>
        ///         Return a <see cref="ByteSize" /> based on a specified number of kilobytes.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI kilobyte (1000) not the traditional kilobyte (1024). For the
        ///         latter, use <see cref="FromKibibytes" />.
        ///     </para>
        /// </summary>
        /// <param name="kilobytes">The number of kilobytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="kilobytes" /> kilobytes.</returns>
        public static ByteSize FromKilobytes (decimal kilobytes) => new ByteSize (kilobytes * ByteSize.BytesInKilobyte) ;

        /// <summary>
        ///     Return a <see cref="ByteSize" /> based on a specified number of kibibytes.
        /// </summary>
        /// <param name="kibibytes">The number of kibibytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="kibibytes" /> kibibytes.</returns>
        public static ByteSize FromKibibytes (decimal kibibytes) => new ByteSize (kibibytes * ByteSize.BytesInKibibyte) ;

        /// <summary>
        ///     <para>
        ///         Return a <see cref="ByteSize" /> based on a specified number of megabytes.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI megabyte (1000^2) not the traditional megabyte (1024^2). For the
        ///         latter, use <see cref="FromMebibytes" />.
        ///     </para>
        /// </summary>
        /// <param name="megabytes">The number of megabytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="megabytes" /> megabytes.</returns>
        public static ByteSize FromMegabytes (decimal megabytes) => new ByteSize (megabytes * ByteSize.BytesInMegabyte) ;

        /// <summary>
        ///     Return a <see cref="ByteSize" /> based on a specified number of mebibytes.
        /// </summary>
        /// <param name="mebibytes">The number of mebibytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="mebibytes" /> mebibytes.</returns>
        public static ByteSize FromMebibytes (decimal mebibytes) => new ByteSize (mebibytes * ByteSize.BytesInMebibyte) ;

        /// <summary>
        ///     <para>
        ///         Return a <see cref="ByteSize" /> based on a specified number of gigabytes.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI gigabyte (1000^3) not the traditional gigabyte (1024^3). For the
        ///         latter, use <see cref="FromGibibytes" />.
        ///     </para>
        /// </summary>
        /// <param name="gigabytes">The number of gigabytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="gigabytes" /> gigabytes.</returns>
        public static ByteSize FromGigabytes (decimal gigabytes) => new ByteSize (gigabytes * ByteSize.BytesInGigabyte) ;

        /// <summary>
        ///     Return a <see cref="ByteSize" /> based on a specified number of gibibytes.
        /// </summary>
        /// <param name="gibibytes">The number of gibibytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="gibibytes" /> gibibytes.</returns>
        public static ByteSize FromGibibytes (decimal gibibytes) => new ByteSize (gibibytes * ByteSize.BytesInGibibyte) ;

        /// <summary>
        ///     <para>
        ///         Return a <see cref="ByteSize" /> based on a specified number of terabytes.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI terabyte (1000^4) not the traditional terabyte (1024^4). For the
        ///         latter, use <see cref="FromTebibytes" />.
        ///     </para>
        /// </summary>
        /// <param name="terabytes">The number of terabytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="terabytes" /> terabytes.</returns>
        public static ByteSize FromTerabytes (decimal terabytes) => new ByteSize (terabytes * ByteSize.BytesInTerabyte) ;

        /// <summary>
        ///     Return a <see cref="ByteSize" /> based on a specified number of tebibytes.
        /// </summary>
        /// <param name="tebibytes">The number of tebibytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="tebibytes" /> tebibytes.</returns>
        public static ByteSize FromTebibytes (decimal tebibytes) => new ByteSize (tebibytes * ByteSize.BytesInTebibyte) ;

        /// <summary>
        ///     <para>
        ///         Return a <see cref="ByteSize" /> based on a specified number of petabytes.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI petabyte (1000^5) not the traditional petabyte (1024^5). For the
        ///         latter, use <see cref="FromPebibytes" />.
        ///     </para>
        /// </summary>
        /// <param name="petabytes">The number of petabytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="petabytes" /> petabytes.</returns>
        public static ByteSize FromPetabytes (decimal petabytes) => new ByteSize (petabytes * ByteSize.BytesInPetabyte) ;

        /// <summary>
        ///     Return a <see cref="ByteSize" /> based on a specified number of pebibytes.
        /// </summary>
        /// <param name="pebibytes">The number of pebibytes.</param>
        /// <returns>A <see cref="ByteSize" /> representing <paramref name="pebibytes" /> pebibytes.</returns>
        public static ByteSize FromPebibytes (decimal pebibytes) => new ByteSize (pebibytes * ByteSize.BytesInPebibyte) ;

        #endregion

        /// <summary>
        ///     Number of bytes represented by this structure.
        /// </summary>
        public decimal Bytes { get ; }

        #region Alternate units

        /// <summary>
        ///     Number of bits represented by this structure.
        /// </summary>

        // ReSharper disable ExceptionNotDocumented
        public long Bits => (this.Bytes * ByteSize.BitsInByte).ConvertTo <long> () ;

        // ReSharper restore ExceptionNotDocumented

        /// <summary>
        ///     <para>
        ///         Number of kilobytes and fractional-kilobytes represented by this structure.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI kilobyte (1000) not the traditional kilobyte (1024). For the
        ///         latter, use <see cref="Kibibytes" />.
        ///     </para>
        /// </summary>
        public decimal Kilobytes => this.Bytes / ByteSize.BytesInKilobyte ;

        /// <summary>
        ///     Number of kibibytes and fractional-kibibytes represented by this structure.
        /// </summary>
        public decimal Kibibytes => this.Bytes / ByteSize.BytesInKibibyte ;

        /// <summary>
        ///     <para>
        ///         Number of megabytes and fractional-megabytes represented by this structure.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI megabyte (1000^2) not the traditional megabyte (1024^2). For the
        ///         latter, use <see cref="Mebibytes" />.
        ///     </para>
        /// </summary>
        public decimal Megabytes => this.Bytes / ByteSize.BytesInMegabyte ;

        /// <summary>
        ///     Number of mebibytes and fractional-mebibytes represented by this structure.
        /// </summary>
        public decimal Mebibytes => this.Bytes / ByteSize.BytesInMebibyte ;

        /// <summary>
        ///     <para>
        ///         Number of gigabytes and fractional-gigabytes represented by this structure.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI gigabyte (1000^3) not the traditional gigabyte (1024^3). For the
        ///         latter, use <see cref="Gibibytes" />.
        ///     </para>
        /// </summary>
        public decimal Gigabytes => this.Bytes / ByteSize.BytesInGigabyte ;

        /// <summary>
        ///     Number of gibibytes and fractional-gibibytes represented by this structure.
        /// </summary>
        public decimal Gibibytes => this.Bytes / ByteSize.BytesInGibibyte ;

        /// <summary>
        ///     <para>
        ///         Number of terabytes and fractional-terabytes represented by this structure.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI terabyte (1000^4) not the traditional terabyte (1024^4). For the
        ///         latter, use <see cref="Tebibytes" />.
        ///     </para>
        /// </summary>
        public decimal Terabytes => this.Bytes / ByteSize.BytesInTerabyte ;

        /// <summary>
        ///     Number of tebibytes and fractional-tebibytes represented by this structure.
        /// </summary>
        public decimal Tebibytes => this.Bytes / ByteSize.BytesInTebibyte ;

        /// <summary>
        ///     <para>
        ///         Number of petabytes and fractional-petabytes represented by this structure.
        ///     </para>
        ///     <para>
        ///         WARNING: This uses the SI petabyte (1000^5) not the traditional petabyte (1024^5). For the
        ///         latter, use <see cref="Pebibytes" />.
        ///     </para>
        /// </summary>
        public decimal Petabytes => this.Bytes / ByteSize.BytesInPetabyte ;

        /// <summary>
        ///     Number of pebibytes and fractional-pebibytes represented by this structure.
        /// </summary>
        public decimal Pebibytes => this.Bytes / ByteSize.BytesInPebibyte ;

        #endregion

        #region Addition

        /// <summary>
        ///     Add two <see cref="ByteSize" />s.
        /// </summary>
        /// <param name="size">The <see cref="ByteSize" /> to add to this one.</param>
        /// <returns>The total size.</returns>
        /// <exception cref="OverflowException"><see cref="ByteSize" /> cannot represent a size that large.</exception>
        public ByteSize Add (ByteSize size)
        {
            decimal newValue = this.Bytes + size.Bytes ;

            if (newValue * ByteSize.BitsInByte > long.MaxValue)
                throw new OverflowException () ;

            return new ByteSize (newValue) ;
        }

        /// <summary>
        ///     Adds a number of bytes to the <see cref="ByteSize" />.
        /// </summary>
        /// <param name="bytes">The number of bytes to add.</param>
        /// <returns>The new size.</returns>
        /// <exception cref="OverflowException"><see cref="ByteSize" /> cannot represent a size that large.</exception>
        public ByteSize Add (decimal bytes)
        {
            decimal newValue = this.Bytes + bytes ;

            if (newValue * ByteSize.BitsInByte > long.MaxValue)
                throw new OverflowException () ;

            return new ByteSize (newValue) ;
        }

        #endregion

        #region Overrides

        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals (object obj)
        {
            if (!(obj is ByteSize))
                return false ;

            return this.Equals ((ByteSize) obj) ;
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode () => this.Bytes.GetHashCode () ;

        /// <summary>
        ///     Returns the value of this <see cref="ByteSize" /> in the default format, in the non-SI unit of
        ///     appropriate size, to two decimal places.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> containing the value of this <see cref="ByteSize" />.
        /// </returns>
        public override string ToString () => this.ToString (null, CultureInfo.CurrentCulture) ;

        #endregion

        #region IComparable<ByteSize> Members

        /// <summary>
        ///     Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has the following
        ///     meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This
        ///     object is equal to <paramref name="other" />. Greater than zero This object is greater than
        ///     <paramref name="other" />.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo (ByteSize other) => this.Bytes.CompareTo (other.Bytes) ;

        #endregion

        #region IEquatable<ByteSize> Members

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals (ByteSize other) => this.Bytes == other.Bytes ;

        #endregion

        #region IFormattable Members

        /// <summary>
        ///     Formats the value of the current instance using the specified format.
        /// </summary>
        /// <returns>
        ///     The value of the current instance in the specified format.
        /// </returns>
        /// <param name="format">
        ///     The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format
        ///     defined for the type of the <see cref="T:System.IFormattable" /> implementation.
        /// </param>
        /// <param name="formatProvider">
        ///     The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to
        ///     obtain the numeric format information from the current locale setting of the operating system.
        /// </param>
        /// <filterpriority>2</filterpriority>
        /// <exception cref="FormatException">Format string was improperly specified.</exception>
        public string ToString (string? format, IFormatProvider? formatProvider)
        {
            // FORMAT STRINGS
            //
            // {1/2/3 x letter code}
            //      G: general format - same as "I" (default)
            //      I: binary units, by size
            //      S: SI units, by size
            //
            //      b: bits
            //      B: bytes with no decimal places
            //      KB: kilobytes
            //      MB: megabytes
            //      GB: gigabytes
            //      TB: terabytes
            //
            //      KiB: kibibytes
            //      MiB: mebibytes
            //      GiB: gibibytes
            //      TiB: tebibytes
            //
            // {number of decimal places - default is two}
            //

            if (format == null)
                format = "G" ;

            if (formatProvider == null)
                formatProvider = CultureInfo.CurrentCulture ;

            string formatStart = format.TrimEnd ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9') ;
            string formatEnd = format.Substring (formatStart.Length) ;

            int decimals ;

            try
            {
                decimals = formatEnd.Length > 0 ? int.Parse (formatEnd) : 2 ;
            }
            catch (OverflowException)
            {
                decimals = 2 ;
            }

            string fmt ;

            switch (formatStart)
            {
                case ByteSize.BitSymbol:

                    // Format as bits. There can be no decimals.
                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                    return ($"{0:D} {ByteSize.BitSymbol}").FillWith (formatProvider, this.Bits) ;

                case ByteSize.ByteSymbol:

                    // Format as bytes.
                    return this.FormatHelper (formatProvider, decimals, ByteSize.ByteSymbol, this.Bytes) ;

                case ByteSize.KilobyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.KilobyteSymbol, this.Kilobytes) ;

                case ByteSize.MegabyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.MegabyteSymbol, this.Megabytes) ;

                case ByteSize.GigabyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.GigabyteSymbol, this.Gigabytes) ;

                case ByteSize.TerabyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.TerabyteSymbol, this.Terabytes) ;

                case ByteSize.PetabyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.PetabyteSymbol, this.Petabytes) ;

                case ByteSize.KibibyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.KibibyteSymbol, this.Kibibytes) ;

                case ByteSize.MebibyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.MebibyteSymbol, this.Mebibytes) ;

                case ByteSize.GibibyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.GibibyteSymbol, this.Gibibytes) ;

                case ByteSize.TebibyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.TebibyteSymbol, this.Tebibytes) ;

                case ByteSize.PebibyteSymbol:
                    return this.FormatHelper (formatProvider, decimals, ByteSize.PebibyteSymbol, this.Pebibytes) ;

                case "S":

                    // Format in SI way.
                    fmt = $"{this.FindLargestSi()}{decimals}" ;
                    return this.ToString (fmt, formatProvider) ;

                case "G":
                case "I":

                    // Format in general/binary way.
                    fmt = $"{this.FindLargestBinary()}{decimals}" ;
                    return this.ToString (fmt, formatProvider) ;

                default:
                    throw new FormatException () ;
            }
        }

        #endregion

        /// <summary>
        ///     Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">
        ///     The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format
        ///     defined for the type of the <see cref="T:System.IFormattable" /> implementation.
        /// </param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString (string? format) => this.ToString (format, CultureInfo.CurrentCulture) ;

        private string FormatHelper (
            IFormatProvider formatProvider,
            int decimals,
            string? suffix,
            decimal amount)
        {
            string formatString = "{{0:N{0}}} {1}".FillWith (formatProvider, decimals, suffix ?? string.Empty) ;

            return formatString.FillWith (formatProvider, amount) ;
        }

        private string FindLargestSi ()
        {
            if (this.Bytes >= ByteSize.BytesInPetabyte)
                return ByteSize.PetabyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInTerabyte)
                return ByteSize.TerabyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInGigabyte)
                return ByteSize.GigabyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInMegabyte)
                return ByteSize.MegabyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInKilobyte)
                return ByteSize.KilobyteSymbol ;
            if (this.Bytes >= 1)
                return ByteSize.ByteSymbol ;

            return ByteSize.BitSymbol ;
        }

        private string FindLargestBinary ()
        {
            if (this.Bytes >= ByteSize.BytesInPebibyte)
                return ByteSize.PebibyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInTebibyte)
                return ByteSize.TebibyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInGibibyte)
                return ByteSize.GibibyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInMebibyte)
                return ByteSize.MebibyteSymbol ;
            if (this.Bytes >= ByteSize.BytesInKibibyte)
                return ByteSize.KibibyteSymbol ;
            if (this.Bytes >= 1)
                return ByteSize.ByteSymbol ;

            return ByteSize.BitSymbol ;
        }

        #region Operators

        /// <summary>
        ///     Addition operator.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>Sum of the operands.</returns>
        public static ByteSize operator + (ByteSize first, ByteSize second)
        {
            decimal newValue = first.Bytes + second.Bytes ;

            if (newValue * ByteSize.BitsInByte > long.MaxValue)
                throw new OverflowException () ;

            return new ByteSize (newValue) ;
        }

        /// <summary>
        ///     Increment operator (one-byte increments).
        /// </summary>
        /// <param name="b">Operand.</param>
        /// <returns>Incremented operand.</returns>
        public static ByteSize operator ++ (ByteSize b)
        {
            decimal newValue = b.Bytes + 1m ;

            if (newValue * ByteSize.BitsInByte > long.MaxValue)
                throw new OverflowException () ;

            return new ByteSize (newValue) ;
        }

        /// <summary>
        ///     Subtraction operator.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>Difference between the operands.</returns>
        public static ByteSize operator - (ByteSize first, ByteSize second)
        {
            decimal newValue = first.Bytes - second.Bytes ;

            return new ByteSize (newValue > 0m ? newValue : 0m) ;
        }

        /// <summary>
        ///     Decrement operator (one-byte decrements).
        /// </summary>
        /// <param name="b">Operand.</param>
        /// <returns>Decremented operand.</returns>
        public static ByteSize operator -- (ByteSize b)
        {
            decimal newValue = b.Bytes - 1 ;

            return new ByteSize (newValue > 0m ? newValue : 0m) ;
        }

        /// <summary>
        ///     Equality operator.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if the operands are equal; false otherwise.</returns>
        public static bool operator == (ByteSize first, ByteSize second) => first.Bytes == second.Bytes ;

        /// <summary>
        ///     Inequality operator.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if the operands are unequal; false otherwise.</returns>
        public static bool operator != (ByteSize first, ByteSize second) => first.Bytes != second.Bytes ;

        /// <summary>
        ///     Less-than operator.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if first operand is less than second operand; false otherwise.</returns>
        public static bool operator < (ByteSize first, ByteSize second) => first.Bytes < second.Bytes ;

        /// <summary>
        ///     Less-than-or-equal operator.
        /// </summary>
        /// <param name="first">First operator.</param>
        /// <param name="second">Second operator.</param>
        /// <returns>True if first operand is less than or equal to second operand; false otherwise.</returns>
        public static bool operator <= (ByteSize first, ByteSize second) => first.Bytes <= second.Bytes ;

        /// <summary>
        ///     Greater-than operator.
        /// </summary>
        /// <param name="first">First operator.</param>
        /// <param name="second">Second operator.</param>
        /// <returns>True if first operand is greater than second operand; false otherwise.</returns>
        public static bool operator > (ByteSize first, ByteSize second) => first.Bytes > second.Bytes ;

        /// <summary>
        ///     Greater-than-or-equal operator.
        /// </summary>
        /// <param name="first">First operator.</param>
        /// <param name="second">Second operator.</param>
        /// <returns>True if first operand is greater than or equal to second operand; false otherwise.</returns>
        public static bool operator >= (ByteSize first, ByteSize second) => first.Bytes >= second.Bytes ;

        #endregion

        #region Parse

        /// <summary>
        ///     Converts the string representation of a data size to its <see cref="ByteSize" /> equivalent.
        /// </summary>
        /// <param name="s">The string representation of the data size to convert.</param>
        /// <returns>The equivalent to the data size contained in <paramref name="s" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" />is null, empty, or whitespace.</exception>
        /// <exception cref="FormatException"><paramref name="s" /> is not in the correct format.</exception>
        /// <exception cref="OverflowException"><paramref name="s" /> is too large to properly represent.</exception>
        public static ByteSize Parse ([Required] string s)
        {
            // Split string into number and symbol.
            // Remove all whitespace.
            s = s.Remove (' ') ;

            // get index of first non-digit (should be start of symbol)
            var decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.ConvertTo <char> () ;
            var groupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator.ConvertTo <char> () ;

            int? split = null ;

            for (var i = 0; i < s.Length; i++)
            {
                if (!(char.IsDigit (s[i]) || (s[i] == decimalSeparator) || (s[i] == groupSeparator)))
                {
                    split = i ;
                    break ;
                }
            }

            if (!split.HasValue)
                throw new FormatException ($@"No byte size (number) found in value '{s}'.") ;

            // Divide the input string
            string numberString = s.Left (split.Value) ;
            string symbol = s.Right (s.Length - split.Value) ;

            // Get the numeric part.
            if (
                !decimal.TryParse (numberString,
                                   NumberStyles.Float | NumberStyles.AllowThousands,
                                   NumberFormatInfo.CurrentInfo,
                                   out var number))

                throw new FormatException ($@"No byte size (number) found in value '{s}'.") ;

            // Get the magnitude part
            // This is case-sensitive, because 'b' and 'B', and because people should be accurate, gorramit.
            switch (symbol)
            {
                case ByteSize.BitSymbol:

                    // No such thing as a partial bit
                    if (number % 1 != 0)
                        throw new FormatException ($@"No such thing as a partial bit in value '{s}'.") ;

                    return ByteSize.FromBits (number.ConvertTo <long> ()) ;

                case ByteSize.ByteSymbol:
                    return ByteSize.FromBytes (number) ;

                case ByteSize.KilobyteSymbol:
                    return ByteSize.FromKilobytes (number) ;

                case ByteSize.KibibyteSymbol:
                    return ByteSize.FromKibibytes (number) ;

                case ByteSize.MegabyteSymbol:
                    return ByteSize.FromMegabytes (number) ;

                case ByteSize.MebibyteSymbol:
                    return ByteSize.FromMebibytes (number) ;

                case ByteSize.GigabyteSymbol:
                    return ByteSize.FromGigabytes (number) ;

                case ByteSize.GibibyteSymbol:
                    return ByteSize.FromGibibytes (number) ;

                case ByteSize.TerabyteSymbol:
                    return ByteSize.FromTerabytes (number) ;

                case ByteSize.TebibyteSymbol:
                    return ByteSize.FromTebibytes (number) ;

                case ByteSize.PetabyteSymbol:
                    return ByteSize.FromPetabytes (number) ;

                case ByteSize.PebibyteSymbol:
                    return ByteSize.FromPebibytes (number) ;

                default:
                    throw new FormatException ($@"No valid byte size (symbol) found in '{s}'.") ;
            }
        }

        /// <summary>
        ///     Converts the string representation of a data size to its <see cref="ByteSize" /> equivalent. A return value
        ///     indicates
        ///     whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">The string representation of the number to convert.</param>
        /// <returns>
        ///     A tuple of true and the <see cref="ByteSize" /> equivalent to the data size contained in <paramref name="s" />,
        ///     if successful. On failure, returns a tuple of false and the zero <see cref="ByteSize" />.
        /// </returns>
        public static (bool success, ByteSize result) TryParse (string? s)
        {
            if (s == null)
                return (false, new ByteSize ()) ;

            try
            {
                ByteSize result = ByteSize.Parse (s) ;
                return (true, result) ;
            }
            catch
            {
                return (false, new ByteSize ()) ;
            }
        }

        #endregion
    }
}

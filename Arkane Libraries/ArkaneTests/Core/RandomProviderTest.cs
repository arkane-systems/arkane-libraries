#region header

// ArkaneTests - RandomProviderTest.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 8:23 PM

#endregion

#region using

using System ;
using System.Threading ;

using ArkaneSystems.Arkane ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core
{
    [TestClass]
    public class RandomProviderTest
    {
        #region Nested type: RandomGrabber

        private class RandomGrabber
        {
            public RandomGrabber (int size, bool keepNumbers)
            {
                this.size = size ;
                if (keepNumbers)
                    this.numbers = new int[size] ;
            }

            private readonly int[] numbers ;
            private readonly int   size ;

            public void GrabNumbers ()
            {
                for (var i = 0; i < this.size; i++)
                {
                    int number = RandomProvider.GetInstance ().Next (100000) ;
                    if (this.numbers != null)
                        this.numbers[i] = number ;
                }
            }

            public override bool Equals (object o)
            {
                var other = (RandomGrabber) o ;
                for (var i = 0; i < this.size; i++)
                {
                    if (this.numbers[i] != other.numbers[i])
                        return false ;
                }

                return true ;
            }

            public override int GetHashCode () => throw new NotImplementedException () ;
        }

        #endregion

        [TestMethod]
        public void CheckDifferentSequences ()
        {
            var grabbers = new RandomGrabber[100] ;
            var threads  = new Thread[grabbers.Length] ;

            for (var i = 0; i < grabbers.Length; i++)
            {
                grabbers[i] = new RandomGrabber (30, true) ;
                threads[i]  = new Thread (grabbers[i].GrabNumbers) ;
            }

            for (var i = 0; i < grabbers.Length; i++)
                threads[i].Start () ;

            for (var i = 0; i < grabbers.Length; i++)
                threads[i].Join () ;

            for (var i = 0; i < grabbers.Length - 1; i++)
            {
                for (int j = i + 1; j < grabbers.Length; j++)
                {
                    if (grabbers[i].Equals (grabbers[j]))
                        Assert.Fail ("Duplicate code sequences retrieved") ;
                }
            }
        }

        [TestMethod]
        public void NextDoubleShouldNotAlwaysReturnInts ()
        {
            // We might get a double like 1.0 *once*, but we're unlikely
            // to get 10 of them unless there's a bug (like the one
            // prompting this test)
            for (var i = 0; i < 10; i++)
            {
                double d = RandomProvider.GetInstance ().NextDouble () ;

                if (Math.Abs ((int) d - d) > 0.01d)
                    return ;
            }

            Assert.Fail ("NextDouble shouldn't just return ints") ;
        }
    }
}

#region header

// ArkaneTests - ByteSizeTests.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 10:01 AM

#endregion

#region using

using ArkaneSystems.Arkane ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core
{
    [TestClass]
    public class ByteSizeTests
    {
        [TestMethod]
        public void Constructor ()
        {
            // Two with one stone.
            var byteSize = 1099511627776L ;

            var result = new ByteSize (byteSize) ;

            // Assert.
            Assert.AreEqual (1099511627776L * 8, result.Bits) ;
            Assert.AreEqual (1099511627776,      result.Bytes) ;
            Assert.AreEqual (1073741824,         result.Kibibytes) ;
            Assert.AreEqual (1048576,            result.Mebibytes) ;
            Assert.AreEqual (1024,               result.Gibibytes) ;
            Assert.AreEqual (1,                  result.Tebibytes) ;
        }

        [TestMethod]
        public void FromBits ()
        {
            long value = 8 ;

            ByteSize result = ByteSize.FromBits (value) ;

            Assert.AreEqual (8, result.Bits) ;
            Assert.AreEqual (1, result.Bytes) ;
        }

        [TestMethod]
        public void FromBytes ()
        {
            // Arrange
            var value = 1.5m ;

            // Act
            ByteSize result = ByteSize.FromBytes (value) ;

            // Assert
            Assert.AreEqual (12,   result.Bits) ;
            Assert.AreEqual (1.5m, result.Bytes) ;
        }

        [TestMethod]
        public void FromKibibytes ()
        {
            // Arrange
            var value = 1.5m ;

            // Act
            ByteSize result = ByteSize.FromKibibytes (value) ;

            // Assert
            Assert.AreEqual (1536, result.Bytes) ;
            Assert.AreEqual (1.5m, result.Kibibytes) ;
        }

        [TestMethod]
        public void FromMebibytes ()
        {
            // Arrange
            var value = 1.5m ;

            // Act
            ByteSize result = ByteSize.FromMebibytes (value) ;

            // Assert
            Assert.AreEqual (1572864, result.Bytes) ;
            Assert.AreEqual (1536,    result.Kibibytes) ;
            Assert.AreEqual (1.5m,    result.Mebibytes) ;
        }

        [TestMethod]
        public void FromGibibytes ()
        {
            // Arrange
            var value = 1.5m ;

            // Act
            ByteSize result = ByteSize.FromGibibytes (value) ;

            // Assert
            Assert.AreEqual (1610612736, result.Bytes) ;
            Assert.AreEqual (1.5m,       result.Gibibytes) ;
        }

        [TestMethod]
        public void FromTebibytes ()
        {
            // Arrange
            var value = 1.5m ;

            // Act
            ByteSize result = ByteSize.FromTebibytes (value) ;

            // Assert
            Assert.AreEqual (1649267441664, result.Bytes) ;
            Assert.AreEqual (1.5m,          result.Tebibytes) ;
        }

        [TestMethod]
        public void ToStringGeneralFormat ()
        {
            ByteSize result = ByteSize.FromMebibytes (1.5m) ;

            string foo = result.ToString () ;

            Assert.AreEqual ("1.50 MiB", foo) ;
        }

        [TestMethod]
        public void ToStringSiFormat ()
        {
            ByteSize result = ByteSize.FromMebibytes (1.5m) ;

            string foo = result.ToString ("S") ;

            Assert.AreEqual ("1.57 MB", foo) ;
        }

        [TestMethod]
        public void ToStringSpecifyPlaces ()
        {
            ByteSize result = ByteSize.FromMebibytes (1.5m) ;

            string foo = result.ToString ("I1") ;

            Assert.AreEqual ("1.5 MiB", foo) ;
        }

        [TestMethod]
        public void ToStringSiSpecifyPlaces ()
        {
            ByteSize result = ByteSize.FromMebibytes (1.5m) ;

            string foo = result.ToString ("S0") ;

            Assert.AreEqual ("2 MB", foo) ;
        }

        [TestMethod]
        public void ToStringBytes ()
        {
            ByteSize result = ByteSize.FromKibibytes (1.5m) ;

            string foo = result.ToString ("B0") ;

            Assert.AreEqual ("1,536 B", foo) ;
        }

        [TestMethod]
        public void ToStringKilobytes ()
        {
            ByteSize result = ByteSize.FromKibibytes (1.5m) ;

            string foo = result.ToString ("KB3") ;

            Assert.AreEqual ("1.536 KB", foo) ;
        }
    }
}

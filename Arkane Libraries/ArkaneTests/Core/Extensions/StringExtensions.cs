#region header

// ArkaneTests - StringExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 10:06 AM

#endregion

#region using

using System ;

using Microsoft.VisualStudio.TestTools.UnitTesting ;

#endregion

namespace ArkaneTests.Core.Extensions
{
    [TestClass]
    public class StringExtTests
    {
        private readonly string sentinelTestString = "the quick brown monkey jumped over the lazy hatfish" ;

        [TestMethod]
        public void EnsureStartsWith_Changing ()
        {
            /* positive testing */
            var extension       = "txt" ;
            var validationValue = ".txt" ;

            // Test with change
            Assert.AreEqual (validationValue, extension.EnsureStartsWith (".")) ;
        }

        [TestMethod]
        public void EnsureStartsWith_NotChanging ()
        {
            var validationValue = ".txt" ;

            // test without change : do nothing
            Assert.AreEqual (validationValue, validationValue.EnsureStartsWith (".")) ;
        }


        [TestMethod]
        public void EnsureEndsWith_Changing ()
        {
            /* positive testing */
            var url             = "http://www.example.com" ;
            var validationValue = "http://www.example.com/" ;
            Assert.AreEqual (validationValue, url.EnsureEndsWith ("/")) ;
        }

        [TestMethod]
        public void EnsureEndsWith_NotChanging ()
        {
            /* positive testing */
            var validationValue = "http://www.example.com/" ;
            Assert.AreEqual (validationValue, validationValue.EnsureEndsWith ("/")) ;
        }

        [TestMethod]
        public void ExpandTabs () { Assert.AreEqual ("        test;", "\t\ttest;".ExpandTabs ()) ; }

        [TestMethod]
        public void FilterIn () { Assert.AreEqual ("hathat", "monkey hat fish monkey hat fish".FilterIn ("hat")) ; }

        [TestMethod]
        public void FilterOut ()
        {
            Assert.AreEqual ("monkey  fish monkey  fish", "monkey hat fish monkey hat fish".FilterOut ("hat")) ;
        }

        [TestMethod]
        public void GetLevenshteinDistance ()
        {
            Assert.AreEqual (0, "".GetLevenshteinDistance ("")) ;
            Assert.AreEqual (5, "".GetLevenshteinDistance ("Tests")) ;
            Assert.AreEqual (5, "Tests".GetLevenshteinDistance ("")) ;
            Assert.AreEqual (1, "Test".GetLevenshteinDistance ("Tests")) ;
            Assert.AreEqual (3, "Test".GetLevenshteinDistance ("Testing")) ;
            Assert.AreEqual (1, "Rest".GetLevenshteinDistance ("Test")) ;
        }

        [TestMethod]
        public void GetOccurrenceCount () { Assert.AreEqual (2, "monkey hat fish monkey hat fish".GetOccurrenceCount ("fish")) ; }

        [TestMethod]
        public void Remove_Strings ()
        {
            var    testValue = "the quick brown fox jumps over the lazy dog." ;
            var    removee   = new[] {"the ", "over ", "brown "} ;
            var    expected  = "quick fox jumps lazy dog." ;
            string result    = testValue.Remove (removee) ;
            Assert.AreEqual (expected, result) ;
        }

        [TestMethod]
        public void Remove_CharsOrDisemvowel ()
        {
            var    testValue = "the quick brown fox jumps over the lazy dog." ;
            var    expected  = "th qck brwn fx jmps vr th lzy dg." ;
            string result    = testValue.Disemvowel () ;
            Assert.AreEqual (expected, result) ;
        }

        [TestMethod]
        public void Reverse_Empty () { Assert.AreEqual (string.Empty, string.Empty.ReverseAsString ()) ; }

        [TestMethod]
        public void Reverse_Normal () { Assert.AreEqual ("!xuuq zab rab ooF", "Foo bar baz quux!".ReverseAsString ()) ; }

        [TestMethod]
        public void Reverse_Combining () { Assert.AreEqual ("selbare\u0301siM seL", "Les Mise\u0301rables".ReverseAsString ()) ; }

        [TestMethod]
        public void AfterSentinel ()
        {
            Assert.AreEqual ("jumped over the lazy hatfish", this.sentinelTestString.AfterSentinel ("monkey")) ;
        }

        [TestMethod]
        public void AfterSentinel_NotFound () { Assert.AreEqual (string.Empty, this.sentinelTestString.AfterSentinel ("spleen")) ; }

        [TestMethod]
        public void BeforeSentinel ()
        {
            Assert.AreEqual ("the quick brown monkey", this.sentinelTestString.BeforeSentinel ("jumped")) ;
        }

        [TestMethod]
        public void BeforeSentinel_NotFound ()
        {
            Assert.AreEqual (this.sentinelTestString, this.sentinelTestString.BeforeSentinel ("spleen")) ;
        }

        [TestMethod]
        public void BetweenSentinels ()
        {
            Assert.AreEqual ("monkey jumped over", this.sentinelTestString.BetweenSentinels ("brown", "the")) ;
        }

        [TestMethod]
        public void BetweenSentinels_LeadingNotFound ()
        {
            Assert.AreEqual (string.Empty, this.sentinelTestString.BetweenSentinels ("spleen", "the")) ;
        }

        [TestMethod]
        public void BetweenSentinels_TrailingNotFound ()
        {
            Assert.AreEqual ("monkey jumped over the lazy hatfish",
                             this.sentinelTestString.BetweenSentinels ("brown", "spleen")) ;
        }

        [TestMethod]
        public void BetweenSentinels_NeitherFound ()
        {
            Assert.AreEqual (string.Empty, this.sentinelTestString.BetweenSentinels ("spleen", "pants")) ;
        }

        [TestMethod]
        [ExpectedException (typeof (ArgumentException))]
        public void BetweenSentinels_Identical () { this.sentinelTestString.BetweenSentinels ("pants", "pants") ; }

        [TestMethod]
        public void Repeat () { Assert.AreEqual ("foofoofoo", "foo".Repeat (3)) ; }

        [TestMethod]
        public void SliceEquals ()
        {
            var test = "foo barbaz" ;
            Assert.IsTrue (test.SliceEquals (0,  "foo ")) ;
            Assert.IsFalse (test.SliceEquals (0, " foo")) ;
            Assert.IsTrue (test.SliceEquals (7,  "baz")) ;
            Assert.IsFalse (test.SliceEquals (7, "bar")) ;
        }

        [TestMethod]
        public void Transform ()
        {
            var    str              = "White Red Blue Green Yellow Black Gray" ;
            var    achromaticColors = new[] {"White", "Black", "Gray"} ;
            string result           = str.Transform (r => r.EqualsAnyOf (achromaticColors), t => "[" + t + "]") ;

            Assert.AreEqual ("[White] Red Blue Green Yellow [Black] [Gray]", result) ;
        }

        [TestMethod]
        public void Wordify_Short ()
        {
            string result = "a".Wordify () ;

            Assert.AreEqual ("A", result, false) ;
        }

        [TestMethod]
        public void Wordify_AllCaps ()
        {
            string result = "ACRONYM".Wordify () ;

            Assert.AreEqual ("ACRONYM", result, false) ;
        }

        [TestMethod]
        public void Wordify_Pascaline ()
        {
            string result = "PascalCase".Wordify () ;

            Assert.AreEqual ("Pascal Case", result, false) ;
        }

        [TestMethod]
        public void Wordify_Bactrian ()
        {
            string result = "camelCase".Wordify () ;

            Assert.AreEqual ("Camel Case", result, false) ;
        }
    }
}

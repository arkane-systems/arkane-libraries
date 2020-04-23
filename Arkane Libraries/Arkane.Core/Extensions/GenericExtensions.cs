#region header

// Arkane.Core - GenericExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 8:04 PM

#endregion



// ReSharper disable once CheckNamespace
namespace System
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static partial class ας_System
#pragma warning restore IDE1006 // Naming Styles
    {
        #region Wrap object in function supplying that object.

        private static T Return <T> (this T value) => value ;

        /// <summary>
        ///     Return a wrapper function which returns a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to wrap.</typeparam>
        /// <param name="value">The object to wrap.</param>
        /// <returns>The wrapper function.</returns>
        /// <remarks>
        ///     Avoids the closure allocation; see https://gist.github.com/AArnott/d285feef75c18f6ecd2b
        /// </remarks>
        public static Func <T> AsFunc <T> (this T value) where T : class => value.Return ;

        #endregion Wrap object in function supplying that object.
    }
}

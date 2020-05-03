#region header

// Arkane.Core - Ref.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 12:24 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

#endregion

namespace ArkaneSystems.Arkane
{
    /// <summary>
    ///     A gettable/settable reference to another variable.
    /// </summary>
    /// <typeparam name="T">The type of the other variable.</typeparam>
    /// <example>
    ///     <code>
    ///         Ref&lt;int&gt; x;
    ///         void M()
    ///         {
    ///             int y = 123;
    ///             x = new Ref&lt;int&gt;(()=>y, z=>{y=z;});
    ///             x.Value = 456;
    ///             Console.WriteLine(y); // 456 -- setting x.Value changes y.
    ///         }
    ///     </code>
    /// </example>
    /// <remarks>
    ///     Based on Eric Lippert's answer here:
    ///     http://stackoverflow.com/questions/2980463/how-do-i-assign-by-reference-to-a-class-field-in-c/2982037#2982037
    /// </remarks>
    [PublicAPI]
    public sealed class Ref <T>
    {
        /// <summary>
        ///     Create a new gettable/settable reference.
        /// </summary>
        /// <param name="getter">The reference's getter function.</param>
        /// <param name="setter">The reference's setter function.</param>
        public Ref (Func <T> getter, Action <T> setter)
        {
            this.getter = getter ;
            this.setter = setter ;
        }

        private readonly Func <T> getter ;

        private readonly Action <T> setter ;

        /// <summary>
        ///     The value of the reference's target.
        /// </summary>
        /// <exception cref="Exception" accessor="get">A delegate callback throws an exception.</exception>
        public T Value { get => this.getter () ; set => this.setter (value) ; }
    }
}

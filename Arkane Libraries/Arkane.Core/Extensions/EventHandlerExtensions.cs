#region header

// Arkane.Core - EventHandlerExtensions.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-02 1:11 PM

#endregion

#region using

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public static partial class ας_System
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        ///     Raise an event if and only if there is a valid event handler registered (i.e. non-null).
        /// </summary>
        /// <param name="handler">The event handler to raise.</param>
        /// <param name="obj">The object on which to raise the event.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void RaiseEvent (this EventHandler? handler,
                                       object?            obj) => handler?.Invoke (obj, EventArgs.Empty) ;

        /// <summary>
        ///     Raise an event if and only if there is a valid event handler registered (i.e. non-null).
        /// </summary>
        /// <typeparam name="T">Type of the event handler to raise.</typeparam>
        /// <param name="handler">The event handler to raise.</param>
        /// <param name="obj">The object on which to raise the event.</param>
        /// <param name="args">The parameters of the event to raise.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void RaiseEvent <T> (this EventHandler <T>? handler,
                                           object?                obj,
                                           T                      args) where T : EventArgs =>
            handler?.Invoke (obj, args) ;

        /// <summary>
        ///     Raise an event if and only if there is a valid event handler registered (i.e. non-null),
        ///     using an <paramref name="argFactory" /> function to create the <see cref="EventArgs" />. This
        ///     function is only called if the event is to be raised; i.e., if there is a handler registered.
        /// </summary>
        /// <typeparam name="T">Type of the event handler to raise.</typeparam>
        /// <param name="handler">The event handler to raise.</param>
        /// <param name="obj">The object on which to raise the event.</param>
        /// <param name="argFactory">A function to create the arguments to the event handler.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void RaiseEventWithArgFactory <T> (this EventHandler <T>? handler,
                                                         object?                obj,
                                                         Func <T>               argFactory)
            where T : EventArgs
        {
            if (handler != null)
            {
                T args = argFactory () ;
                handler (obj, args) ;
            }
        }
    }
}

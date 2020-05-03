#region header

// Arkane.Core - ApartmentAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 7:38 AM

#endregion

#region using

using System ;
using System.Threading ;

using ArkaneSystems.Arkane.Threading ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect to execute a function on a thread in the given COM apartment state.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [LinesOfCodeAvoided (15)]
    public sealed class ApartmentAttribute : MethodInterceptionAspect
    {
        /// <summary>
        ///     An aspect to execute a function on a thread in the given COM apartment state.
        /// </summary>
        /// <param name="state">The desired COM apartment state (default STA).</param>
        public ApartmentAttribute (ApartmentState state = ApartmentState.STA) => this.desiredState = state ;

        [NonSerialized]
        private readonly ApartmentState desiredState ;

        [NonSerialized]
        private Exception? returnException ;

        /// <summary>
        ///     Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        /// <exception cref="ThreadStateException">The thread has already been started.</exception>
        /// <exception cref="CrossThreadException">An exception marshaled across from a worker thread.</exception>
        /// <exception cref="OutOfMemoryException">There is not enough memory available to start the worker thread. </exception>
        /// <exception cref="ThreadInterruptedException">The worker thread is interrupted while waiting. </exception>
        public override void OnInvoke (MethodInterceptionArgs args)
        {
            // Obtain current thread's apartment state.
            ApartmentState currentState = Thread.CurrentThread.GetApartmentState () ;

            if (currentState == this.desiredState)
            {
                // If the current state is the desired state, just proceed.
                args.Proceed () ;
            }
            else
            {
                this.returnException = null ;

                // Spawn new thread to run args.Proceed, putting it in the desired apartment state.
                var aptThread = new Thread (() => this.ProceedOnOtherThread (args)) ;
#pragma warning disable PC001 // API not supported on all platforms
                aptThread.SetApartmentState (this.desiredState) ;
#pragma warning restore PC001 // API not supported on all platforms

                // Run it and block until it is complete.
                aptThread.Start () ;
                aptThread.Join () ;

                // Marshal exceptions back to this thread.
                if (this.returnException != null)
                    throw new CrossThreadException (this.returnException) ;
            }
        }

        private void ProceedOnOtherThread (MethodInterceptionArgs args)
        {
            try
            {
                args.Proceed () ;
            }

            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                this.returnException = ex ;
            }
        }
    }
}

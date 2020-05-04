#region header

// Arkane.Core - RetryAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 1:05 AM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Logging ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects
{
    /// <summary>
    ///     An aspect to retry a function a given number of times, or until it succeeds.
    /// </summary>
    [LinesOfCodeAvoided (16)]
    [PSerializable]
    [PublicAPI]
    public sealed class RetryAttribute : MethodInterceptionAspect
    {
        /// <summary>
        ///     An aspect to retry a function a given number of times, or until it succeeds.
        /// </summary>
        /// <param name="retries">The number of retries that will be attempted (default 3).</param>
        /// <param name="logFailuresAt">The <see cref="LogLevel" /> to log failures at.</param>
        public RetryAttribute (int retries = 3, LogLevel logFailuresAt = LogLevel.Debug)
        {
            this.MaxRetries    = retries ;
            this.LogFailuresAt = logFailuresAt ;
        }

        /// <summary>
        ///     The number of retries that will be attempted.
        /// </summary>
        public int MaxRetries { get ; private set ; }

        /// <summary>
        ///     The number of retries that will be attempted.
        /// </summary>
        public LogLevel LogFailuresAt { get ; private set ; }

        /// <summary>
        ///     Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnInvoke ([NotNull] MethodInterceptionArgs args)
        {
            int retries   = this.MaxRetries ;
            var succeeded = false ;

            while (!succeeded)
            {
                try
                {
                    args.Proceed () ;
                    succeeded = true ;
                }

                // ReSharper disable once CatchAllClause
                catch (Exception ex)
                {
                    // Don't throw until the retry limit is reached.
                    if (retries >= 0)
                    {
                        LogProvider.GetLogger (args.Method.DeclaringType, "RetryAttribute")
                                   .Log (this.LogFailuresAt,
                                         () => "Caught exception '{0}'; retrying ({1}).",
                                         null,
                                         ex.Message,
                                         retries) ;

                        retries-- ;
                    }
                    else

                        // ReSharper disable once ExceptionNotDocumented
                        // ReSharper disable once ThrowingSystemException
                    {
                        throw ;
                    }
                }
            }
        }
    }
}

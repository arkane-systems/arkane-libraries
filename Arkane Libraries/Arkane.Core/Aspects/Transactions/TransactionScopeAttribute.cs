#region header

// Arkane.Core - TransactionScopeAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 8:12 AM

#endregion

#region using

using System ;
using System.Transactions ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Transactions
{
    // TODO: check interrelations with TransactionAttribute

    /// <summary>
    ///     An aspect to place the enclosed method inside a <see cref="System.Transactions.TransactionScope" />.
    /// </summary>
    [PublicAPI]
    [ProvideAspectRole (StandardRoles.TransactionHandling)]
    [Serializable]
    public class TransactionScopeAttribute : MethodInterceptionAspect
    {
        /// <summary>
        ///     Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnInvoke ([NotNull] MethodInterceptionArgs args)
        {
            // Start new transaction.
            using var scope = new TransactionScope () ;

            args.Proceed () ;

            // complete transaction if we haven't thrown before this point.
            scope.Complete () ;
        }
    }
}

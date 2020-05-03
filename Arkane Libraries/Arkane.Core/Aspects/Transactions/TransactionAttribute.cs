#region header

// Arkane.Core - TransactionAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 8:02 AM

#endregion

#region using

using System ;
using System.Diagnostics.CodeAnalysis ;
using System.Reflection ;
using System.Transactions ;

using ArkaneSystems.Arkane.Logging ;
using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Transactions
{
    /// <summary>
    ///     An instance-scoped aspect to wrap a method on a System.Transactions.CommittedTransaction, rolling
    ///     back automatically on exception.
    /// </summary>
    /// <remarks>
    ///     The method in question can get the transaction from <see cref="Transaction.Current" />. This is
    ///     null if there is presently no ambient transaction, and so can be used safely even without this aspect in play.
    /// </remarks>
    [SuppressMessage ("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [CLSCompliant (false)]
    [ProvideAspectRole (StandardRoles.TransactionHandling)]
    [PublicAPI]
    [Serializable]
    public sealed class TransactionAttribute : OnMethodBoundaryAspect, IInstanceScopedAspect
    {
        /// <summary>
        ///     An aspect to wrap a method on a System.Transactions.CommittedTransaction, rolling back automatically on
        ///     exception.
        /// </summary>
        /// <remarks>
        ///     The method in question can get the transaction from <see cref="System.Transactions.Transaction.Current" />. This is
        ///     null if there is presently no ambient transaction, and so can be used safely even without this aspect in play.
        /// </remarks>
        /// <param name="isolationLevel">The isolation level of the transaction.</param>
        /// <param name="timeout">The timeout after which the transaction will automatically abort.</param>
        public TransactionAttribute (IsolationLevel isolationLevel = IsolationLevel.Serializable,
                                     TimeSpan       timeout        = new TimeSpan ())
        {
            this.log = LogProvider.For <TransactionAttribute> () ;

            this.isolationLevel = isolationLevel ;
            this.timeOut        = timeout ;
        }

        [NonSerialized]
        private readonly IsolationLevel isolationLevel ;

        [NonSerialized]
        private readonly ILog log ;

        [NonSerialized]
        private readonly TimeSpan timeOut ;

        private string? className ;
        private string? methodName ;

        [NonSerialized]
        private CommittableTransaction? transaction ;

        /// <inheritdoc />
        public override void CompileTimeInitialize ([System.Diagnostics.CodeAnalysis.NotNull]
                                                    MethodBase method,
                                                    AspectInfo aspectInfo)
        {
            this.className  = method.DeclaringType?.Name ;
            this.methodName = method.Name ;
        }

        /// <inheritdoc />
        /// <exception cref="T:System.PlatformNotSupportedException">
        ///     An attempt to create a transaction under Windows 98, Windows
        ///     98 Second Edition or Windows Millennium Edition.
        /// </exception>
        public override void OnEntry (MethodExecutionArgs args)
        {
            var topts = new TransactionOptions {IsolationLevel = this.isolationLevel, Timeout = this.timeOut} ;

            // Begin the transaction.
            this.transaction    = new CommittableTransaction (topts) ;
            Transaction.Current = this.transaction ;

            this.log.Trace (string.Format (Resources.TransactionAttribute_OnEntry_InitiatedTransaction,
                                           this.className,
                                           this.methodName,
                                           this.transaction.TransactionInformation.LocalIdentifier)) ;
        }

        /// <exception cref="TransactionInDoubtException">
        ///     <see cref="M:System.Transactions.CommittableTransaction.Commit" /> is called on a
        ///     transaction and the transaction becomes <see cref="F:System.Transactions.TransactionStatus.InDoubt" />.
        /// </exception>
        /// <exception cref="TransactionAbortedException">
        ///     <see cref="M:System.Transactions.CommittableTransaction.Commit" /> is called and
        ///     the transaction rolls back for the first time.
        /// </exception>
        public override void OnSuccess (MethodExecutionArgs args)
        {
            // Commit the transaction on success.
            this.transaction!.Commit () ;

            this.log.Trace (string.Format (Resources.TransactionAttribute_OnSuccess_CommittedTransaction,
                                           this.className,
                                           this.methodName,
                                           this.transaction.TransactionInformation.LocalIdentifier)) ;

            this.transaction.Dispose () ;
            Transaction.Current = null ;
        }

        /// <inheritdoc />
        public override void OnException ([System.Diagnostics.CodeAnalysis.NotNull]
                                          MethodExecutionArgs args)
        {
            // Rollback the transaction on exception.
            this.transaction!.Rollback (args.Exception) ;

            this.log.Trace (string.Format (Resources.TransactionAttribute_OnException_RolledBackTransaction,
                                           this.className,
                                           this.methodName,
                                           this.transaction.TransactionInformation.LocalIdentifier)) ;

            this.transaction.Dispose () ;
            Transaction.Current = null ;
        }

        #region IInstanceScopedAspect Members

        /// <inheritdoc />
        public object CreateInstance (AdviceArgs adviceArgs) => this.MemberwiseClone () ;

        /// <inheritdoc />
        public void RuntimeInitializeInstance () { }

        #endregion
    }
}

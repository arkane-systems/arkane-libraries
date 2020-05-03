#region header

// Arkane.Core - FutureAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 4:25 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Patterns.Contracts ;
using PostSharp.Reflection ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Contracts
{
    /// <summary>
    ///     Custom aspect that, when added to a field, property, or parameter of type <see cref="DateTime" />, throws an
    ///     <see cref="ArgumentException" /> if the target is assigned a time not in the future relative to the current time.
    /// </summary>
    [ProvideAspectRole (StandardRoles.Validation)]
    [CLSCompliant (false)]
    [LinesOfCodeAvoided (2)]
    [PublicAPI]
    public sealed class FutureAttribute : LocationContractAttribute, ILocationValidationAspect <DateTime>
    {
        /// <summary>
        ///     Custom aspect that, when added to a field, property, or parameter of type <see cref="DateTime" />, throws an
        ///     <see cref="ArgumentException" /> if the target is assigned a time not in the future relative to the current time.
        /// </summary>
        /// <param name="dayGranularity">Use day granularity; i.e., the future begins with tomorrow.</param>
        public FutureAttribute (bool dayGranularity = false) => this.DayGranularity = dayGranularity ;

        /// <summary>
        ///     Is day granularity enabled? I.e., does the past begin with yesterday.
        /// </summary>
        private bool DayGranularity { get ; }

        #region ILocationValidationAspect<DateTime> Members

        /// <inheritdoc />
        public Exception? ValidateValue (DateTime                  value,
                                         string                    locationName,
                                         LocationKind              locationKind,
                                         LocationValidationContext context)
        {
            if (this.DayGranularity)
            {
                // Compare with granularity of one day
                if (value >= DateTime.Today.AddDays (1))
                    return null ;
            }
            else
            {
                // Raw compare
                if (value > DateTime.Now)
                    return null ;
            }

            if (locationKind == LocationKind.ReturnValue)
                return this.CreatePostconditionFailedException (value, locationName, locationKind) ;

            return this.CreateArgumentException (value, locationName, locationKind) ;
        }

        #endregion

        #region Overrides of LocationContractAttribute

        /// <inheritdoc />
        [JetBrains.Annotations.NotNull]
        protected override string GetErrorMessage ()
            =>
                this.DayGranularity
                    ? "DateTime {0} is not tomorrow or later (value={3})."
                    : "DateTime {0} is not in the future (value={3})." ;

        #endregion
    }
}

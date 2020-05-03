#region header

// Arkane.Core - StringManipulationAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 12:29 PM

#endregion

#region using

using System ;

using ArkaneSystems.Arkane.Properties ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Aspects.Dependencies ;
using PostSharp.Extensibility ;
using PostSharp.Reflection ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Aspects.Data
{
    /// <summary>
    ///     Aspect that properly formats string values.
    /// </summary>
    [AspectRoleDependency (AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.DataBinding)]
    [PSerializable]
    [PublicAPI]
    public sealed class StringManipulationAttribute : LocationInterceptionAspect
    {
        /// <summary>
        ///     Aspect that properly formats string values.
        /// </summary>
        public StringManipulationAttribute ()
        {
            this.ForceTo               = PreferredCase.Unchanged ;
            this.Nullable              = true ;
            this.ConvertNullToEmpty    = false ;
            this.RemoveNonAlphanumeric = false ;
            this.RemoveCrlf            = false ;
            this.RemoveSpaces          = false ;
            this.MaxLength             = -1 ;
        }

        /// <summary>
        ///     Indicates how the string should be capitalized when a new value is assigned to the property.
        /// </summary>
        public PreferredCase ForceTo { get ; set ; }

        /// <summary>
        ///     Indicates if a null value can be assigned to the property.
        /// </summary>
        public bool Nullable { get ; set ; }

        /// <summary>
        ///     Indicates if a null value can be assigned to the property.
        /// </summary>
        public bool RemoveNonAlphanumeric { get ; set ; }

        /// <summary>
        ///     Indicates if the carriage returns and line feeds should be removed.
        /// </summary>
        public bool RemoveCrlf { get ; set ; }

        /// <summary>
        ///     Indicates if the spaces should be removed.
        /// </summary>
        public bool RemoveSpaces { get ; set ; }

        /// <summary>
        ///     Indicates if the value should be trimmed at the start.
        /// </summary>

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool TrimStart { get ; set ; }

        /// <summary>
        ///     Indicates if the value should be trimmed at the end.
        /// </summary>

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool TrimEnd { get ; set ; }

        /// <summary>
        ///     Indicates if a null value assigned to the property should be converted to an empty string.
        /// </summary>
        public bool ConvertNullToEmpty { get ; set ; }

        /// <summary>
        ///     The maximum length of the string. New values will be trimmed to accomodate this.
        /// </summary>
        public int MaxLength { get ; set ; }

        /// <exception cref="InvalidAnnotationException">
        ///     StringManipulationAttribute can only be applied to locations of type
        ///     string.
        /// </exception>
        public override bool CompileTimeValidate (LocationInfo locationInfo)
        {
            if (locationInfo.LocationType != typeof (string))
                throw new InvalidAnnotationException (
                                                      Resources
                                                         .StringManipulationAttribute_CompileTimeValidate_CanOnlyBeUsedOnString) ;

            return true ;
        }

        /// <inheritdoc />
        public override void OnSetValue (LocationInterceptionArgs args)
        {
            string? newValue = this.ConvertString (args) ;
            args.Value = newValue ;
            args.ProceedSetValue () ;
        }

        private string? ConvertString (LocationInterceptionArgs args)
        {
            string? newValue = args.Value == null ? null : Convert.ToString (args.Value) ;
            if (newValue == null)
            {
                if (this.ConvertNullToEmpty)
                    newValue = string.Empty ;
                else if (!this.Nullable)
                    throw new ArgumentNullException (
                                                     string.Format (Resources
                                                                       .StringManipulationAttribute_ConvertString_CannotBeNull,
                                                                    args.LocationName)) ;
            }
            else
            {
                if (this.RemoveNonAlphanumeric)
                    newValue = newValue.RemoveNonAlphaNumeric () ;

                switch (this.ForceTo)
                {
                    case PreferredCase.Uppercase:
                        newValue = newValue.ToUpper () ;
                        break ;
                    case PreferredCase.Lowercase:
                        newValue = newValue.ToLower () ;
                        break ;
                    case PreferredCase.Sentence:
                        if (newValue.Length > 1)
                            newValue = newValue.Substring (0, 1).ToUpper () + newValue.Substring (1).ToLower () ;
                        else
                            newValue = newValue.ToUpper () ;

                        break ;
                }
            }

            string? res ;
            if ((newValue != null) && (this.MaxLength > 0))
                res = newValue.UpTo (this.MaxLength, string.Empty) ;
            else
                res = newValue ;

            if (res != null)
            {
                if (this.RemoveCrlf)
                    res = res.Replace ("\r", string.Empty).Replace ("\n", string.Empty) ;

                if (this.RemoveSpaces)
                    res = res.Replace (" ", string.Empty) ;

                if (this.TrimStart)
                    res = res.TrimStart () ;

                if (this.TrimEnd)
                    res = res.TrimEnd () ;

                return res ;
            }

            return null ;
        }
    }
}

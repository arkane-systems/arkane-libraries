#region header

// Arkane.Data - EntityConstraintAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-03 12:58 PM

#endregion

#region using

using System ;
using System.ComponentModel.DataAnnotations ;
using System.ComponentModel.DataAnnotations.Schema ;
using System.Linq ;
using System.Reflection ;

using ArkaneSystems.Arkane.Data.Properties ;

using JetBrains.Annotations ;

using PostSharp.Constraints ;
using PostSharp.Extensibility ;

#endregion

namespace ArkaneSystems.Arkane.Data
{
    /// <summary>
    ///     A constraint which verifies that the class to which it is applied obeys the Entity Framework entity pattern
    ///     we use.
    /// </summary>
    /// <remarks>
    ///     The entity pattern requires:
    ///     * type is public, non-static, not sealed
    ///     * public int Id { get; set; } property, with attributes [Key], [Required], [DatabaseGenerated
    ///     (DatabaseGeneratedOption.Identity)]
    ///     * public byte[] Version { get; set; } property, with [Timestamp] attribute
    /// </remarks>
    [MulticastAttributeUsage (MulticastTargets.Class, Inheritance = MulticastInheritance.None)]
    [PublicAPI]
    public sealed class EntityConstraintAttribute : ScalarConstraint
    {
        /// <summary>
        ///     Validates the element of code to which the constraint is applied.
        /// </summary>
        /// <param name="target">
        ///     Element of code to which the constraint is applied (<see cref="T:System.Reflection.Assembly" />,
        ///     <see cref="T:System.Type" />,
        ///     <see cref="T:System.Reflection.MethodInfo" />, <see cref="T:System.Reflection.ConstructorInfo" />,
        ///     <see cref="T:System.Reflection.PropertyInfo" />,
        ///     <see cref="T:System.Reflection.EventInfo" />, <see cref="T:System.Reflection.FieldInfo" />,
        ///     <see cref="T:System.Reflection.ParameterInfo" />).
        /// </param>
        /// <exception cref="TypeLoadException">A custom attribute type could not be loaded. </exception>
        public override void ValidateCode (object target)
        {
            var      targetType = (Type) target ;
            TypeInfo ttInfo     = targetType.GetTypeInfo () ;

            //     * type is public, non-static, not sealed
            if (!ttInfo.IsPublic)
                Message.Write (targetType,
                               SeverityType.Error,
                               "EC001",
                               Resources.EntityConstraintAttribute_ValidateCode_EntityTypeNotPublic,
                               targetType.Name) ;

            if (ttInfo.IsSealed)
                Message.Write (targetType,
                               SeverityType.Error,
                               "EC002",
                               Resources.EntityConstraintAttribute_ValidateCode_EntityTypeCannotBeSealedOrStatic,
                               targetType.Name) ;

            //     * public int Id { get; set; } property, with attributes [Key], [Required], [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
            PropertyInfo? idProperty = ttInfo.DeclaredProperties
                                             .Where (p => p.GetMethod.IsPublic  &&
                                                          !p.GetMethod.IsStatic &&
                                                          p.SetMethod.IsPublic  &&
                                                          !p.SetMethod.IsStatic)
                                             .Where (p => p.Name == "Id")
                                             .Where (p => p.CanRead)
                                             .Where (p => p.CanWrite)
                                             .SingleOrDefault (p => p.PropertyType == typeof (int)) ;

            if (idProperty != null)
            {
                CustomAttributeData[] attrs = idProperty.CustomAttributes.ToArray () ;

                if (attrs.SingleOrDefault (a => a.AttributeType == typeof (KeyAttribute)) == null)
                    Message.Write (targetType,
                                   SeverityType.Error,
                                   "EC004",
                                   Resources.EntityConstraintAttribute_ValidateCode_EntityIdMissingKeyAttribute,
                                   targetType.Name) ;

                if (attrs.SingleOrDefault (a => a.AttributeType == typeof (RequiredAttribute)) == null)
                    Message.Write (targetType,
                                   SeverityType.Error,
                                   "EC005",
                                   Resources.EntityConstraintAttribute_ValidateCode_EntityIdMissingRequiredAttribute,
                                   targetType.Name) ;

                CustomAttributeData? dbga = attrs.SingleOrDefault (a => a.AttributeType == typeof (DatabaseGeneratedAttribute)) ;

                if ((dbga == null) ||
                    ((DatabaseGeneratedOption) dbga.ConstructorArguments
                                                   .SingleOrDefault (ca => ca.ArgumentType == typeof (DatabaseGeneratedOption))
                                                   .Value !=
                     DatabaseGeneratedOption.Identity))

                    //(dbga.DatabaseGeneratedOption != DatabaseGeneratedOption.Identity))
                    Message.Write (targetType,
                                   SeverityType.Error,
                                   "EC006",
                                   Resources.EntityConstraintAttribute_ValidateCode_EntityTypeMissingIncorrectDatabaseGenerated,
                                   targetType.Name) ;
            }
            else
            {
                Message.Write (targetType,
                               SeverityType.Error,
                               "EC003",
                               Resources.EntityConstraintAttribute_ValidateCode_EntityTypeMissingId,
                               targetType.Name) ;
            }

            //     * public byte[] Version { get; set; } property, with [Timestamp] attribute
            PropertyInfo? versionProperty = ttInfo.DeclaredProperties
                                                  .Where (p => p.GetMethod.IsPublic  &&
                                                               !p.GetMethod.IsStatic &&
                                                               p.SetMethod.IsPublic  &&
                                                               !p.SetMethod.IsStatic)
                                                  .Where (p => p.Name == "Version")
                                                  .Where (p => p.CanRead)
                                                  .Where (p => p.CanWrite)
                                                  .SingleOrDefault (p => p.PropertyType == typeof (byte[])) ;

            if (versionProperty != null)
            {
                if (versionProperty.GetCustomAttributes (false).SingleOrDefault (a => a is TimestampAttribute) == null)
                    Message.Write (targetType,
                                   SeverityType.Error,
                                   "EC008",
                                   Resources.EntityConstraintAttribute_ValidateCode_EntityVersionMissingTimestamp,
                                   targetType.Name) ;
            }
            else
            {
                Message.Write (targetType,
                               SeverityType.Error,
                               "EC007",
                               Resources.EntityConstraintAttribute_ValidateCode_EntityTypeMissingVersion,
                               targetType.Name) ;
            }
        }
    }
}

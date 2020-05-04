#region header

// ArkaneTests - Entity.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-05-04 1:36 AM

#endregion

#region using

using System.ComponentModel.DataAnnotations ;
using System.ComponentModel.DataAnnotations.Schema ;

using ArkaneSystems.Arkane.Data ;

#endregion

namespace ArkaneTests.Data
{
    [EntityConstraint]
    public class Entity
    {
        [Key]
        [Required]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public int Id { get ; set ; }

        [Timestamp]
        public byte[] Version { get ; set ; }
    }
}

#region header

// Arkane.Core - ModuleInit.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2020.  All rights reserved.
// 
// Created: 2020-04-22 3:26 PM

#endregion

#region using

using System ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;

#endregion

namespace ArkaneSystems.Arkane.Internal
{
    /// <summary>
    ///     Module initializer. Marked static method runs whenever this module is initialized.
    /// </summary>
    public static class ModuleInit
    {
        /// <summary>
        ///     Module initializer. Runs whenever this module is initialized.
        /// </summary>
        [ModuleInitializer (0)]
        [UsedImplicitly]
        public static void InitializeModule ()
        {
            // Mark our presence in the AppContext.
            AppContext.SetSwitch ("Switch.ArkaneSystems.Arkane.Core.Presence", true) ;
        }
    }
}

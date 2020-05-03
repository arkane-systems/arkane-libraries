#region header

// Arkane Libraries - SharedProperties.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 6:15 PM

#endregion

#region using

using System ;
using System.Runtime.CompilerServices ;
using System.Runtime.InteropServices ;
using System.Security.Cryptography ;
using System.Security.Cryptography.X509Certificates ;

using ArkaneSystems.Arkane.Annotations ;

#endregion

// CLS Compliance
[assembly: CLSCompliant (true)]

// Author and documentation information.
// Author information
[assembly: Author ("Alistair J. R. Young", "avatar@arkane-systems.net")]
[assembly: Documentation ("https://arkane-libraries.readthedocs.io/en/latest/")]

// Language information
[assembly: SourceLanguage (ProgrammingLanguages.CSharp, RewrittenByPostSharp = true)]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible (false)]

// Version information
[assembly: AddGitStamp]

// Allow tests to see internals.
[assembly: InternalsVisibleTo ("ArkaneTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100394a42792741f9788ef7e0cbf9ed64cab054729206938220a4564c1106ed68c118fbd5396bca31df269201dd5e95592e9eff6dfc18dbb552a45337c7cf920e5de8500ef540ac0a21c49fa0ae7bf68887517a0f74d9ce94a82011c79ad01ac999ac83990c0c157346dbbac64cd7d5a14384fbdf0c73fa9bd5aba2d21bf5a2d6e1")]

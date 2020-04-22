#region header

// Arkane.Annotations - Threading.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2020-04-22 1:07 AM

#endregion


#region using

using PostSharp.Patterns.Threading ;

#endregion

// [assembly: DeadlockDetectionPolicy]
[assembly: ThreadSafetyPolicy]

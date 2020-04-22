#region header

// Arkane.Annotations - AddGitStampAttribute.cs
// 
// Alistair J. R. Young
// Arkane Systems
// 
// Copyright Arkane Systems 2012-2019.  All rights reserved.
// 
// Created: 2019-05-19 7:19 PM

#endregion

#region using

using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Reflection ;

using JetBrains.Annotations ;

using PostSharp.Aspects ;
using PostSharp.Extensibility ;
using PostSharp.Reflection ;
using PostSharp.Serialization ;

#endregion

namespace ArkaneSystems.Arkane.Annotations
{
    /// <summary>
    ///     An attribute marking an assembly with precise version information derived from its
    ///     git repository. Specifically, it extracts the git information from disk, combines it
    ///     with the assembly version, and places it in the <see cref="AssemblyGitVersionAttribute" />.
    /// </summary>
    /// <remark>
    ///     The function of this attribute/aspect was derived from that of the Fody equivalent, Fody.Stamp
    ///     ( https://github.com/Fody/Stamp ), although independently implemented.
    /// </remark>
    [AttributeUsage (AttributeTargets.Assembly)]
    [LinesOfCodeAvoided (0)]
    [PSerializable]
    [PublicAPI]
    public sealed class AddGitStampAttribute : AssemblyLevelAspect, IAspectProvider
    {
        private string? gitVersion ;

        #region IAspectProvider Members

        /// <summary>
        ///     Provides new aspects.
        /// </summary>
        /// <param name="targetElement">
        ///     Code element (<see cref="T:System.Reflection.Assembly" />, <see cref="T:System.Type" />,
        ///     <see cref="T:System.Reflection.FieldInfo" />, <see cref="T:System.Reflection.MethodBase" />,
        ///     <see cref="T:System.Reflection.PropertyInfo" />, <see cref="T:System.Reflection.EventInfo" />,
        ///     <see cref="T:System.Reflection.ParameterInfo" />, or <see cref="T:PostSharp.Reflection.LocationInfo" />) to which
        ///     the current aspect has been applied.
        /// </param>
        /// <returns>
        ///     A set of aspect instances.
        /// </returns>
        [ItemNotNull]
        public IEnumerable <AspectInstance> ProvideAspects (object targetElement)
        {
            var targetAssembly = (Assembly) targetElement ;

            string infoVersion = $"{targetAssembly.GetName ().Version} {this.gitVersion}" ;

            // Add the AssemblyGitVersionAttribute.
            var agvaConstruction =
                new ObjectConstruction (typeof (AssemblyGitVersionAttribute),
                                        infoVersion) ;

            var introduceAssemblyGitVersionAttribute = new CustomAttributeIntroductionAspect (agvaConstruction) ;

            yield return new AspectInstance (targetAssembly, introduceAssemblyGitVersionAttribute) ;
        }

        #endregion

        /// <summary>
        ///     Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked
        ///     before any other build-time method.
        /// </summary>
        /// <param name="assembly">Assembly to which the current aspect is applied</param>
        /// <param name="aspectInfo">Reserved for future usage.</param>
        public override void CompileTimeInitialize (Assembly assembly, AspectInfo aspectInfo)
        {
            // Get the project path
            string projectPath =
                Path.GetDirectoryName (
                                       PostSharpEnvironment.CurrentProject.EvaluateExpression (
                                                                                               "{$MSBuildProjectFullPath}")) ??
                "." ;

            // Get the current branch and commit.
            var gitLogStart = new ProcessStartInfo (@"git.exe", @"log -1 --format=""%d %H""")
                              {
                                  WorkingDirectory       = projectPath,
                                  RedirectStandardOutput = true,
                                  UseShellExecute        = false,
                                  CreateNoWindow         = true
                              } ;

            var gitStatusStart = new ProcessStartInfo (@"git.exe", @"status --porcelain --untracked-files=no")
                                 {
                                     WorkingDirectory       = projectPath,
                                     RedirectStandardOutput = true,
                                     UseShellExecute        = false,
                                     CreateNoWindow         = true
                                 } ;

            try
            {
                Process gitLog = Process.Start (gitLogStart) ;

                Debug.Assert (gitLog != null, "gitLog != null") ;

                string output = gitLog.StandardOutput.ReadToEnd () ;
                gitLog.WaitForExit () ;

                if (gitLog.ExitCode != 0)
                {
                    Message.Write (assembly,
                                   SeverityType.Error,
                                   "GS0001",
                                   "Git log did not exit cleanly, exit code = {0}",
                                   gitLog.ExitCode) ;
                    return ;
                }

                this.gitVersion = output.Trim () ;

                Process gitStatus = Process.Start (gitStatusStart) ;

                Debug.Assert (gitStatus != null, "gitStatus != null") ;

                string output2 = gitStatus.StandardOutput.ReadToEnd () ;
                gitStatus.WaitForExit () ;

                if (gitStatus.ExitCode != 0)
                {
                    Message.Write (assembly,
                                   SeverityType.Error,
                                   "GS0003",
                                   "Git status did not exit cleanly, exit code = {0}",
                                   gitStatus.ExitCode) ;
                    return ;
                }

                string[] lines = output2.Split (new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries) ;

                if (lines.Any ())
                    this.gitVersion += " modified" ;

                Message.Write (assembly, SeverityType.Info, "GS0004", "Lines: {0}", lines.Length) ;
            }

            // ReSharper disable once CatchAllClause
            catch (Exception ex)
            {
                Message.Write (assembly,
                               SeverityType.Error,
                               "GS0002",
                               "An exception was thrown attempting to run git, message = {0}",
                               ex.Message) ;
            }
        }
    }
}

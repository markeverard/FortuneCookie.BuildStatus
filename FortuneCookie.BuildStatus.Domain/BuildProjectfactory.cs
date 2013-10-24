using System;

namespace FortuneCookie.BuildStatus.Domain
{
    /// <summary>
    /// Factory method for creating BuildProjects
    /// </summary>
    public static class BuildProjectFactory
    {
        public static BuildProject Create(BuildState status, string name, string url, string[] breakers, string fixer, DateTime lastBuildTime)
        {
            return new BuildProject {BuildStatus = status, Name = name, Url = url, Breakers = breakers, Fixer = fixer, LastBuildTime = lastBuildTime };
        }
         
        public static BuildProject CreateNotConnectedBuild()
        {
             return new BuildProject {BuildStatus = BuildState.NotConnected, Name = "Unconnected", LastBuildTime = DateTime.MinValue };
        }
    }
}
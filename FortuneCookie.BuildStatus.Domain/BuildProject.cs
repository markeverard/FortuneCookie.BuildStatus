using System;

namespace FortuneCookie.BuildStatus.Domain
{
    /// <summary>
    /// Class describing a Build project
    /// </summary>
    public class BuildProject
    {
        public BuildState BuildStatus { get; set; }
        public string[] Breakers { get; set; }
        public string Fixer { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime LastBuildTime { get; set; }

        public bool HasFixer
        {
            get { return !string.IsNullOrEmpty(Fixer); }
        }
    }
}
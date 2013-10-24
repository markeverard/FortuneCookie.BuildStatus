using System;

namespace FortuneCookie.BuildStatus.Domain
{
    /// <summary>
    /// Class describing a build
    /// </summary>
    public class BuildDetails
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime Date { get; set; }
        public int NumberChangedFiles { get; set; }
        public string VersionNumber { get; set; }
        public BuildState Status { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FortuneCookie.BuildStatus.Domain.BuildStatusProviders
{
    /// <summary>
    /// Interface describing the build status provider - use this to extend for CI build systems
    /// </summary>
    public interface IBuildStatusProvider
    {  
        IEnumerable<BuildProject> GetBuildProjects();
        IEnumerable<BuildDetails> GetBuildDetails();

        IEnumerable<DataServiceDetails> DataServiceDetails();
        
        BuildState Status();
        IEnumerable<BuildProject> BrokenProjects();
        IEnumerable<BuildProject> BuildingProjects();

        int BrokenProjectCount { get; }
        int TotalProjectCount { get; }

        IEnumerable<BuildDetails> DailyBuildDetails(DateTime date);
        DailyBuildSummary DailySummary(DateTime date);

    }
}
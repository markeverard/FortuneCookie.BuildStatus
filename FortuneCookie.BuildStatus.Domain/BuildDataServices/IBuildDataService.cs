using System;
using System.Collections.Generic;

namespace FortuneCookie.BuildStatus.Domain.BuildDataServices
{
    /// <summary>
    /// Interface describing the details a build status dataService must return
    /// </summary>
    public interface IBuildDataService
    {
        DataServiceDetails ServiceDetails { get;}
        IEnumerable<BuildProject> GetBuildProjects(Guid cacheKey);
        IEnumerable<BuildDetails> GetBuildDetails(Guid cacheKey);
        Guid UniqueServiceIdentifier { get; }
    }
}
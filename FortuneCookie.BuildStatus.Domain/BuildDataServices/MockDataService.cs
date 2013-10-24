using System;
using System.Collections.Generic;

namespace FortuneCookie.BuildStatus.Domain.BuildDataServices
{
    public class MockDataService : IBuildDataService
    {
        public MockDataService()
        {
            ServiceDetails = new DataServiceDetails(string.Empty, "Mock");
            UniqueServiceIdentifier = Guid.NewGuid();
            TimeZoneOffset = ServiceDetails.TimeZoneOffset;
        }

        public Guid UniqueServiceIdentifier { get; private set; }

        public TimeSpan TimeZoneOffset { get; set; }

        public DataServiceDetails ServiceDetails { get; set; }

       public IEnumerable<BuildProject> GetBuildProjects(Guid cachekey)
        {
            return new List<BuildProject> {BuildProjectFactory.CreateNotConnectedBuild()};
        }

       public IEnumerable<BuildDetails> GetBuildDetails(Guid cachekey)
        {
            return new List<BuildDetails>
                           {
                               new BuildDetails
                                   {
                                       Date = DateTime.Now,
                                       Id = Guid.NewGuid(),
                                       NumberChangedFiles = 10,
                                       ProjectName = "Mock Project",
                                       Status = BuildState.NotConnected,
                                       VersionNumber = "1.0.0.0"
                                   }
                           };
        }
    }
}
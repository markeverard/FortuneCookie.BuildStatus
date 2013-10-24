using System;
using System.Collections.Generic;
using System.Linq;
using FortuneCookie.BuildStatus.Domain.BuildDataServices;

namespace FortuneCookie.BuildStatus.Domain.BuildStatusProviders
{
    public class BuildStatusProvider : IBuildStatusProvider
    {
        public BuildStatusProvider(IBuildDataService dataService)
        {
            DataService = dataService;
        }

        public IEnumerable<BuildProject> GetBuildProjects()
        {
            return DataService.GetBuildProjects(DataService.UniqueServiceIdentifier);
        }

        public IEnumerable<BuildDetails> GetBuildDetails()
        {
            return DataService.GetBuildDetails(DataService.UniqueServiceIdentifier);
        }


        public IBuildDataService DataService { get; set; }

        public IEnumerable<DataServiceDetails> DataServiceDetails()
        {
            return new List<DataServiceDetails> { DataService.ServiceDetails };
        }

        public BuildState Status()
        {
            var projects = GetBuildProjects().ToList();

            if (projects.Any(b => b.BuildStatus == BuildState.NotConnected))
                return BuildState.NotConnected;

            if (projects.Any(b => b.BuildStatus == BuildState.Building))
                return BuildState.Building;

            if (projects.Any(b => b.BuildStatus == BuildState.Broken || b.BuildStatus == BuildState.Exception))
                return BuildState.Broken;

            return BuildState.Good;
        }

        public IEnumerable<BuildProject> BrokenProjects()
        {
            return GetBuildProjects().Where(b => b.BuildStatus == BuildState.Broken);
        }

        public IEnumerable<BuildProject> BuildingProjects()
        {
            return GetBuildProjects().Where(b => b.BuildStatus == BuildState.Building);
        }

        public int BrokenProjectCount
        {
            get { return BrokenProjects().Count(); }
        }

        public int TotalProjectCount
        {
            get { return GetBuildProjects().Count(); }
        }

        public IEnumerable<BuildDetails> DailyBuildDetails(DateTime date)
        {
            return GetBuildDetails().Where(b => b.Date.Date == date.Date);
        }

        public DailyBuildSummary DailySummary(DateTime date)
        {
            var dailyBuilds = DailyBuildDetails(date).ToList();
            return DailyBuildSummaryFactory.Create(date, dailyBuilds);
        }
    }
}
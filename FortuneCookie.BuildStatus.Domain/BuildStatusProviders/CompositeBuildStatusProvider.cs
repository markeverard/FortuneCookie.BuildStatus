using System;
using System.Collections.Generic;
using System.Linq;

namespace FortuneCookie.BuildStatus.Domain.BuildStatusProviders
{
    /// <summary>
    /// CompositeBuildStatusProvider - that collects the results from multiply defined IBuildStatusProviders
    /// </summary>
    public class CompositeBuildStatusProvider : IBuildStatusProvider
    {
        public IEnumerable<IBuildStatusProvider> BuildStatusProviders { get; internal set; } 
        
        public CompositeBuildStatusProvider(IEnumerable<IBuildStatusProvider> buildStatusProviders)
        {
            BuildStatusProviders = buildStatusProviders;
        }

        public IEnumerable<DataServiceDetails> DataServiceDetails()
        {
            return BuildStatusProviders.SelectMany(b => b.DataServiceDetails());
        }

        public BuildState Status()
        {
            var statuses = BuildStatusProviders.Select(b => b.Status()).ToList();

            if (statuses.Contains(BuildState.NotConnected))
                return BuildState.NotConnected;
            
            if (statuses.Contains(BuildState.Building))
                return BuildState.Building;

            if (statuses.Contains(BuildState.Broken))
                return BuildState.Broken;

            return BuildState.Good;
        }

        public IEnumerable<BuildProject> GetBuildProjects()
        {
            return BuildStatusProviders.SelectMany(b => b.GetBuildProjects()); 
        }

        public IEnumerable<BuildDetails> GetBuildDetails()
        {
            return BuildStatusProviders.SelectMany(b => b.GetBuildDetails());
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
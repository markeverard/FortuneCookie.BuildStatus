using System;
using System.Collections.Generic;
using System.Linq;

namespace FortuneCookie.BuildStatus.Domain
{
    public static class DailyBuildSummaryFactory
    {
        public static DailyBuildSummary Create(DateTime date, IList<BuildDetails> dailyBuilds)
        {
            return new DailyBuildSummary()
            {
                Date = date,
                FailedBuilds = dailyBuilds.Count(b => b.Status == BuildState.Broken),
                TotalBuilds = dailyBuilds.Count(),
                NumberChangedFiles = dailyBuilds.Sum(b => b.NumberChangedFiles)
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using FortuneCookie.BuildStatus.Domain;
using FortuneCookie.BuildStatus.Domain.BuildStatusProviders;

namespace FortuneCookie.BuildStatus.Web.Models
{
    public class BuildDetailsViewModel
    {
        public BuildDetailsViewModel(IBuildStatusProvider buildStatusProvider)
        {
            Details = buildStatusProvider.GetBuildDetails().OrderByDescending(d => d.Date).Take(13).ToList();
            DailySummary = buildStatusProvider.DailySummary(DateTime.Today.AddDays(-1));
        }

        public DailyBuildSummary DailySummary { get; set; }
        public IList<BuildDetails> Details { get; set; }
    }
}
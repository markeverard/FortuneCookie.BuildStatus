using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FortuneCookie.BuildStatus.Domain;
using FortuneCookie.BuildStatus.Domain.BuildStatusProviders;

namespace FortuneCookie.BuildStatus.Web.Models
{
    public class BuildStatusViewModel
    {
        public BuildStatusViewModel(IBuildStatusProvider buildStatusProvider)
        {
            var status = buildStatusProvider.Status();
            CurrentBuildStatus = status.ToString();
            DetailsViewModel = new BuildDetailsViewModel(buildStatusProvider);
            ServiceDetails = buildStatusProvider.DataServiceDetails().ToList();

            if (status == BuildState.NotConnected) 
                return;

            BrokenProjects = buildStatusProvider.BrokenProjects().ToList();
            BuildingProjects = buildStatusProvider.BuildingProjects().ToList();
        
            BrokenProjectCount = buildStatusProvider.BrokenProjectCount.ToString();
            TotalProjectCount = buildStatusProvider.TotalProjectCount.ToString();
        }

        public string CurrentBuildStatus { get; set; }
        
        public IList<BuildProject> BrokenProjects { get; set; }
        public IList<BuildProject> BuildingProjects { get; set; }

        public string BrokenProjectCount { get; set; }
        public string TotalProjectCount { get; set; }

        public BuildDetailsViewModel DetailsViewModel { get; set; }

        [UIHint("ListDataServiceModel")]
        public IList<DataServiceDetails> ServiceDetails { get; set; } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ThoughtWorks.CruiseControl.Remote;

namespace FortuneCookie.BuildStatus.Domain.Convertors
{
    /// <summary>
    /// Converts ProjectStatus objects to strong-typed domain BuildProjects objects 
    /// </summary>
    public class CruiseControlClientConvertor
    {
        public CruiseControlClientConvertor(TimeSpan timeZoneOffset)
        {
            TimeZoneOffset = timeZoneOffset;
        }

        protected TimeSpan TimeZoneOffset { get; set; }

        public IEnumerable<BuildProject> ToBuildProjects(IEnumerable<ProjectStatus> projects)
        {
            foreach (var project in projects)
            {
                if (project.BuildStatus == IntegrationStatus.Unknown)
                    yield return BuildProjectFactory.CreateNotConnectedBuild();
                
                var status = ParseToBuildStatus(project.BuildStatus, project.Activity);
                var name = project.Name;
                var url = project.WebURL;
                var messages = project.Messages;
                var lastBuildDate = project.LastBuildDate.Add(TimeZoneOffset);

               
                var breakersList = messages.Where(m => m.Kind == Message.MessageKind.Breakers);
                var breakers = breakersList.Select(m => m.ToString()).ToArray();

                var fixersList = messages.Where((m => m.Kind == Message.MessageKind.Fixer));
                var fixer = fixersList.Select(m => m.ToString()).ToString();

                yield return BuildProjectFactory.Create(status, name, url, breakers, fixer, lastBuildDate);
            }
        }

        private BuildState ParseToBuildStatus(IntegrationStatus status, ProjectActivity activity)
        {
            if (activity == ProjectActivity.Building)
                return BuildState.Building;
            
            if (status == IntegrationStatus.Success)
                return BuildState.Good;

            return BuildState.Broken;
        }
    }
}
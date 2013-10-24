using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using FortuneCookie.Aspects.Attributes;
using FortuneCookie.Aspects.Caching;
using FortuneCookie.BuildStatus.Domain.Convertors;
using ThoughtWorks.CruiseControl.Remote;

namespace FortuneCookie.BuildStatus.Domain.BuildDataServices
{
    public class CruiseControlHttpClientService : IBuildDataService
    {
        public CruiseControlHttpClientService(DataServiceDetails serviceDetails)
        {
            ServiceDetails = serviceDetails;
            Parser = new CruiseControlClientConvertor(ServiceDetails.TimeZoneOffset);
            RssConvertor = new BuildRssFeedConvertor(ServiceDetails.TimeZoneOffset);
            UniqueServiceIdentifier = Guid.NewGuid();
        }

        public Guid UniqueServiceIdentifier { get; private set; }

        public DataServiceDetails ServiceDetails { get; internal set; }
        protected CruiseControlClientConvertor Parser { get; set; }
        protected BuildRssFeedConvertor RssConvertor { get; set; }

        [MethodCache(CacheLocation = CacheLocation.ContextItems)]
        public virtual IEnumerable<BuildProject> GetBuildProjects(Guid cacheKey)
        {
            ProjectStatus[] projectStatuses;
               
            using (var client = new CruiseServerHttpClient(ServiceDetails.DataAddress))
            {
                try
                {
                    projectStatuses = client.GetProjectStatus();
                }
                catch (WebException)
                {
                    //time-out
                    projectStatuses = new[] { new ProjectStatus("Unconnected", IntegrationStatus.Unknown, DateTime.Now) };
                }
            }

            return Parser.ToBuildProjects(projectStatuses); 
        }

        [MethodCache(CacheLocation = CacheLocation.ContextItems)]
        public virtual IEnumerable<BuildDetails> GetBuildDetails(Guid cacheKey)
        {
            IEnumerable<BuildProject> projects = GetBuildProjects(cacheKey);
            var builds = new List<BuildDetails>();

            var client = new CruiseServerHttpClient(ServiceDetails.DataAddress);

            foreach (var project in projects)
            {
                try
                {
                    var rss1 = client.GetRSSFeed(project.Name);
                    XDocument rss = XDocument.Load(rss1);
                    builds.AddRange(RssConvertor.GetBuildFromXml(rss));
                }
                catch (Exception)
                {
                    //time-out
                }
            }

            return builds;
        }
    }
}
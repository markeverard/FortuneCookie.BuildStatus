using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using FortuneCookie.Aspects.Attributes;
using FortuneCookie.Aspects.Caching;
using FortuneCookie.BuildStatus.Domain.Convertors;

namespace FortuneCookie.BuildStatus.Domain.BuildDataServices
{
    /// <summary>
    /// Dataservice returning data via a http web client
    /// </summary>
    public class HttpDataService : IBuildDataService
    {
        public HttpDataService(DataServiceDetails serviceDetails)
        {
            ServiceDetails = serviceDetails;
            Convertor = new CruiseControlDashboardParser(ServiceDetails.TimeZoneOffset);
            RssConvertor = new BuildRssFeedConvertor(ServiceDetails.TimeZoneOffset);
            UniqueServiceIdentifier = Guid.NewGuid();
        }

        public Guid UniqueServiceIdentifier { get; private set; }

        public DataServiceDetails ServiceDetails { get; internal set; }
        
        protected CruiseControlDashboardParser Convertor { get; set; }
        protected BuildRssFeedConvertor RssConvertor { get; set; }

        /// <summary>
        /// Gets strongly typed Build details object from the http serivce defined in ServiceDetails.
        /// </summary>
        /// <returns></returns>
        [MethodCache(CacheLocation = CacheLocation.ContextItems)]
        public virtual IEnumerable<BuildProject> GetBuildProjects(Guid cachekey)
        {
            string data;

            using (var client = new WebClientTimeOut())
            {
                try
                {
                    data = client.DownloadString(ServiceDetails.DataAddress);
                }
                catch (WebException)
                {
                    //time-out
                    data = string.Empty;
                }
            }
            return Convertor.ToBuildProjects(data);
        }

        /// <summary>
        /// Gets the BuildDetails from service http sevice specified in ServiceDetails.
        /// </summary>
        /// <returns></returns>
        [MethodCache(CacheLocation = CacheLocation.ContextItems)]
        public virtual IEnumerable<BuildDetails> GetBuildDetails(Guid cachekey)
        {
            var builds = new List<BuildDetails>();

            IEnumerable<BuildProject> projects = GetBuildProjects(cachekey).ToList();
            if (projects.Any(p => p.BuildStatus == BuildState.NotConnected))
                return builds;

            foreach (string rssUrl in projects.Select(CreateRssUrl))
            {
                try
                {
                    XDocument rss = XDocument.Load(rssUrl);
                    builds.AddRange(RssConvertor.GetBuildFromXml(rss));
                }
                catch (Exception)
                {
                    //time-out
                }
            }

            return builds;
        }

        internal string CreateRssUrl(BuildProject buildProject)
        {
            if (buildProject == null)
                return string.Empty;

            if (string.IsNullOrEmpty(buildProject.Url))
                return string.Empty;


            string relativeRssUrl = buildProject.Url.Replace("ViewProjectReport.aspx", "RssFeed.aspx");
            string serviceUrl = ServiceDetails.DataAddress.Replace("/ViewFarmReport.aspx", string.Empty);
            return string.Concat(serviceUrl, relativeRssUrl);
        }
    }
}
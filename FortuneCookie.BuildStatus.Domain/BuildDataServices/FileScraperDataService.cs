using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FortuneCookie.Aspects.Attributes;
using FortuneCookie.Aspects.Caching;
using FortuneCookie.BuildStatus.Domain.Convertors;

namespace FortuneCookie.BuildStatus.Domain.BuildDataServices
{
    public class FileScraperDataService : IBuildDataService
    {
        public FileScraperDataService(DataServiceDetails serviceModel)
        {
            ServiceDetails = serviceModel;
            Convertor = new CruiseControlDashboardParser(ServiceDetails.TimeZoneOffset);
            RssConvertor = new BuildRssFeedConvertor(ServiceDetails.TimeZoneOffset);
            UniqueServiceIdentifier = Guid.NewGuid();
        }

        public Guid UniqueServiceIdentifier { get; private set; }
      
        public DataServiceDetails ServiceDetails { get; internal set; }
        protected CruiseControlDashboardParser Convertor { get; set; }
        protected BuildRssFeedConvertor RssConvertor { get; set; }

        [MethodCache(CacheLocation = CacheLocation.ContextItems)]
        public virtual IEnumerable<BuildProject> GetBuildProjects(Guid cachekey)
        {
            string data;

            using (var streamReader = new StreamReader(ServiceDetails.DataAddress))
            {
                data = streamReader.ReadToEnd();
            }
            
            return Convertor.ToBuildProjects(data); 
        }

        [MethodCache(CacheLocation = CacheLocation.ContextItems)]
        public virtual IEnumerable<BuildDetails> GetBuildDetails(Guid cachekey)
        {
            var rssFile = ServiceDetails.DashboardAddress;

            using (var streamReader = new StreamReader(rssFile))
            {
                XDocument rss = XDocument.Parse(streamReader.ReadToEnd());
                return RssConvertor.GetBuildFromXml(rss);
            }
        }
    }
}
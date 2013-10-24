using System;
using System.Collections.Generic;
using FortuneCookie.BuildStatus.Domain.Convertors;

namespace FortuneCookie.BuildStatus.Domain.BuildDataServices
{
    public class CruiseControlRemotingDataService : IBuildDataService
    {
        public CruiseControlRemotingDataService(DataServiceDetails serviceDetails)
        {
            ServiceDetails = serviceDetails;
            Parser = new CruiseControlClientConvertor(ServiceDetails.TimeZoneOffset);
            UniqueServiceIdentifier = Guid.NewGuid();
        }

        public Guid UniqueServiceIdentifier { get; private set; }
       
        public DataServiceDetails ServiceDetails { get; internal set; }
        protected CruiseControlClientConvertor Parser { get; set; }

        public IEnumerable<BuildProject> GetBuildProjects(Guid cacheKey)
        {
            throw new NotImplementedException();
            //var factory = new RemoteCruiseManagerFactory();
            //ICruiseServerClient client = factory.GetCruiseServerClient(ServiceDetails.DataAddress);
            //ProjectStatus[] projectStatuses = client.GetProjectStatus(new ServerRequest(s))
            //return Parser.ToBuildProjects();
        }

        public IEnumerable<BuildDetails> GetBuildDetails(Guid cacheKey)
        {
            throw new NotImplementedException();
        }
    }
}
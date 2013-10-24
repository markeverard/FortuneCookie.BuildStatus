using FortuneCookie.BuildStatus.Domain;
using FortuneCookie.BuildStatus.Domain.BuildDataServices;
using FortuneCookie.BuildStatus.Domain.Convertors;
using NUnit.Framework;

namespace FortuneCookie.BuildStatus.Test.Domain
{
    [TestFixture]
    public class HttpDataService_Test
    {
        private HttpDataService _httpDataService;

        [SetUp]
        public void InitTest()
        {
            var serviceDetails = new DataServiceDetails("test", "http://build.fcdev.local/ViewFarmReport.aspx");
            _httpDataService = new HttpDataService(serviceDetails);
        }

        [Test]
        public void Can_Create_Full_RSS_Feed_URL()
        {
            const string expected = "http://build.fcdev.local/Server/Project/RssFeed.aspx";
            var buildProject = new BuildProject() {Url = "/Server/Project/ViewProjectReport.aspx"};
            string actual = _httpDataService.CreateRssUrl(buildProject);
            Assert.AreEqual(expected, actual);
        }
    }
}
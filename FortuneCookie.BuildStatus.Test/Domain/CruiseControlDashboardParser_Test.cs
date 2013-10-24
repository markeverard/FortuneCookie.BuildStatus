using System;
using System.Collections.Generic;
using System.Linq;
using FortuneCookie.BuildStatus.Domain;
using FortuneCookie.BuildStatus.Domain.BuildDataServices;
using FortuneCookie.BuildStatus.Domain.BuildStatusProviders;
using NUnit.Framework;

namespace FortuneCookie.BuildStatus.Test.Domain
{
    [TestFixture]
    public class CruiseControlDashboardParser_Test
    {
        [SetUp]
        public void InitTest()
        {
            var serviceDetails = new DataServiceDetails("File", @"..\..\ExampleData\BrokenScreen.html")
                                     {TimeZoneOffset = new TimeSpan(-1, 0, 0)};
            var fileDataService = new FileScraperDataService(serviceDetails);
            StatusProvider = new BuildStatusProvider(fileDataService);
        }

        protected IBuildStatusProvider StatusProvider;
        
        [Test]    
        public void Build_Is_Broken()
        {
            Assert.IsTrue(StatusProvider.Status() == BuildState.Broken);
        }

        [Test]
        public void Build_Is_Good()
        {
            var serviceDetails = new DataServiceDetails("File", @"..\..\ExampleData\GoodScreen.html");
            StatusProvider = new BuildStatusProvider(new FileScraperDataService(serviceDetails));
            Assert.IsTrue(StatusProvider.Status() == BuildState.Good);
        }

        [Test]
        public void Build_Is_Building()
        {
            var serviceDetails = new DataServiceDetails("File", @"..\..\ExampleData\BuildingScreen.html");
            StatusProvider = new BuildStatusProvider(new FileScraperDataService(serviceDetails));
            Assert.IsTrue(StatusProvider.Status() == BuildState.Building);
        }

        [Test]
        public void Build_Is_NotConnected()
        {
            var serviceDetails = new DataServiceDetails("File", @"..\..\ExampleData\NotConnected.html");
            StatusProvider = new BuildStatusProvider(new FileScraperDataService(serviceDetails));
            Assert.IsTrue(StatusProvider.Status() == BuildState.NotConnected);
        }

        [Test]
        public void Build_Is_Broken_And_Contains_Me_In_List_Of_Breakers()
        {
            Assert.IsTrue(StatusProvider.Status() == BuildState.Broken);
            Assert.IsTrue(StatusProvider.BrokenProjects().SelectMany(p => p.Breakers).Contains("mark.everard"));
        }

        [Test]
        public void Build_Is_Broken_And_Contains_Me_In_List_Of_Fixers()
        {
            Assert.IsTrue(StatusProvider.Status() == BuildState.Broken);

            var expectedFixers = StatusProvider.BrokenProjects().Select(p => p.Fixer);
            Assert.IsTrue(expectedFixers.Contains("mark.everard1"));
        }

        [Test]
        public void Can_Parse_LastBuildTime_With_Adjusted_TimeSpan()
        {
            var expectedDatedate = new DateTime(2011, 8, 18, 11, 2, 15);
            var firstBrokenBuild = StatusProvider.GetBuildProjects().First(b => b.BuildStatus == BuildState.Broken);
            Assert.IsTrue(firstBrokenBuild.LastBuildTime == expectedDatedate);
        }

        [Test]
        public void Can_Parse_WebUrl()
        {
            const string expectedUrl = "/server/local/project/AmexMerchant%20Create%20Package/ViewProjectReport.aspx";
            var firstBrokenBuild = StatusProvider.GetBuildProjects().First(b => b.BuildStatus == BuildState.Broken);
            Assert.IsTrue(firstBrokenBuild.Url == expectedUrl);
        }

    }
}
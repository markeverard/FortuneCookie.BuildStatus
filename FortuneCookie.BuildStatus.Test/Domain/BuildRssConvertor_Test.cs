using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FortuneCookie.BuildStatus.Domain;
using FortuneCookie.BuildStatus.Domain.Convertors;
using NUnit.Framework;

namespace FortuneCookie.BuildStatus.Test.Domain
{
    [TestFixture]
    public class BuildRssConvertor_Test
    {
        private static IEnumerable<BuildDetails> ParseBuildFromRssFile()
        {
            var timeZoneOffset = new TimeSpan(-1, 0, 0);
            var convertor = new BuildRssFeedConvertor(timeZoneOffset);
            XDocument rss = XDocument.Load(@"..\..\ExampleData\RSSFeedExample.xml");
            var builds = convertor.GetBuildFromXml(rss);
            return builds;
        }

        [Test]
        public void Can_Read_Two_Entry_From_RssFeed()
        {
             var builds = ParseBuildFromRssFile();
             Assert.IsTrue(builds.Count() == 2);
        }
        
        [Test]
        public void Can_Parse_Build_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().FirstOrDefault();
            Assert.IsTrue(build != null);
         }

        [Test]
        public void Can_Parse_Build_Id_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().FirstOrDefault();
            Assert.IsTrue(build.Id == new Guid("96228163-d6ca-4f4c-9196-15d5ee5619c8"));
        }

        [Test]
        public void Can_Parse_Build_NumberChangedFiles_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().FirstOrDefault();
            Assert.IsTrue(build.NumberChangedFiles == 83);
        }

        [Test]
        public void Can_Parse_Build_NoChangedFiles_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().LastOrDefault();
            Assert.IsTrue(build.NumberChangedFiles == 0);
        }


        [Test]
        public void Can_Parse_Build_VersionNumber_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().FirstOrDefault();
            Assert.IsTrue(build.VersionNumber == "1.0.0.151");
        }

        [Test]
        public void Can_Parse_Build_Status_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().FirstOrDefault();
            Assert.IsTrue(build.Status == BuildState.Broken);
        }

        [Test]
        public void Can_Parse_Build_Date_With_Adjusted_TimeSpan_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().FirstOrDefault();
            Assert.IsTrue(build.Date == new DateTime(2011, 10, 13, 16, 45, 19));
        }

        [Test]
        public void Can_Parse_Build_ProjectName_From_RssFeed()
        {
            var build = ParseBuildFromRssFile().FirstOrDefault();
            Assert.IsTrue(build.ProjectName == "De Beers Corporate Site Publishing");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FortuneCookie.BuildStatus.Domain.Convertors
{
    /// <summary>
    /// Converts RSSFeeds to strongly typed domain BuildDetails objects
    /// </summary>
    public class BuildRssFeedConvertor
    {
        public BuildRssFeedConvertor(TimeSpan timeZoneOffset)
        {
            TimeZoneOffset = timeZoneOffset;
        }

        protected TimeSpan TimeZoneOffset { get; set; }

        public IEnumerable<BuildDetails> GetBuildFromXml(XDocument xdoc)
        {
            var channel = GetChannelQuery(xdoc).FirstOrDefault();
            if (channel == null)
                return new List<BuildDetails>(); 

            return channel.Items.Select(item => new BuildDetails
                                                    {
                                                        ProjectName = channel.Title.Remove(0, 20),
                                                        Date = DateTime.Parse(item.PubDate).Add(TimeZoneOffset),
                                                        NumberChangedFiles = ParseNumberChangedFiles(item.Description),
                                                        Id = new Guid(item.Guid),
                                                        Status = ParseStatus(item.Title),
                                                        VersionNumber = ParseVersionNumber(item.Title)
                                                    });
        }

        private string ParseVersionNumber(string title)
        {
            int firstIndexOfColon = title.IndexOf(':');
            return title.Substring(0, firstIndexOfColon - 1).Replace("Build", string.Empty).Trim();
        }

        private BuildState ParseStatus(string title)
        {
            int firstIndexOfColon = title.IndexOf(':');
            var stripLeft = title.Substring(firstIndexOfColon + 2);

            var status = stripLeft.Split(' ').First();

            if (status == "Failure")
                return BuildState.Broken;

            return BuildState.Good;
        }

        private int ParseNumberChangedFiles(string description)
        {
            string changedFiles = description.Split(' ').First();
            return changedFiles == "No" 
                ? 0 
                : int.Parse(changedFiles);
        }

        private IEnumerable<Channel> GetChannelQuery(XDocument xdoc)
        {
            return from channels in xdoc.Descendants("channel")
                   select new Channel
                   {
                       Title = channels.Element("title") != null ? channels.Element("title").Value : "",
                       Items = from items in channels.Descendants("item")
                               select new Item
                               {
                                   Title = items.Element("title") != null ? items.Element("title").Value : "",
                                   Link = items.Element("link") != null ? items.Element("link").Value : "",
                                   Description = items.Element("description") != null ? items.Element("description").Value : "",
                                   Guid = (items.Element("guid") != null ? items.Element("guid").Value : ""),
                                   PubDate = (items.Element("pubDate") != null ? items.Element("pubDate").Value : "")
                               }
                   };
        }

        private class Channel
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
            public IEnumerable<Item> Items { get; set; }
        }

        private class Item
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
            public string Guid { get; set; }
            public string PubDate { get; set; }
        }
    }
}
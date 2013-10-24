using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace FortuneCookie.BuildStatus.Domain.Convertors
{
    /// <summary>
    /// Converts flat html page to strong-typed domain BuildProjects objects 
    /// </summary>
    public class CruiseControlDashboardParser
    {
        public CruiseControlDashboardParser(TimeSpan timeZoneOffset)
        {
            TimeZoneOffset = timeZoneOffset;
        }

        protected TimeSpan TimeZoneOffset { get; set; }

        public IEnumerable<BuildProject> ToBuildProjects(string inputString)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(inputString);

            var statusTableRows = doc.DocumentNode.SelectNodes("//table[@id='StatusGrid']//tr");

            if (statusTableRows != null)
            {
                foreach (HtmlNode element in statusTableRows)
                {
                    var dataCells = element.ChildNodes.Where(s => s.Name == "td").ToList();

                    if (dataCells.Count != 10)
                        continue;

                    string name = dataCells[1].InnerText.Trim();
                    string url = dataCells[1].ChildNodes[1].Attributes["href"].Value.Trim();
                    string status = dataCells[2].InnerText.Trim();
                    string activity = dataCells[7].InnerText.Trim();
                    string breakers = "build.server";
                    string fixer = string.Empty;

                    var buildMessages = dataCells[8].Descendants().Where(s => s.Name == "li");

                    foreach (var message in buildMessages)
                    {
                        string messageText = message.InnerText;

                        if (messageText.StartsWith("Breakers"))
                            breakers = messageText.Split(':')[1].Trim();

                        if (messageText.EndsWith(" is fixing the build."))
                        {
                            fixer = messageText
                                .Substring(0, messageText.LastIndexOf(" is fixing the build."))
                                .Trim();
                        }
                    }
                   
                    var lastBuildTimeString = dataCells[3].InnerText.Trim();
                    var lastBuildTime = DateTime.Parse(lastBuildTimeString).Add(TimeZoneOffset);

                    var buildStatus = ParseToBuildStatus(status, activity);

                    yield return BuildProjectFactory.Create(buildStatus, name, url, breakers.Split(','), fixer, lastBuildTime);
                }
            }
            else
            {
                yield return BuildProjectFactory.CreateNotConnectedBuild();
            }
        }

        private BuildState ParseToBuildStatus(string status, string activity)
        {
            if (activity == "Building")
                return BuildState.Building;

            if (status == "Unknown")
                return BuildState.Good;

            if (status == "Success")
                return BuildState.Good;

            if (status == "Failure")
                return BuildState.Broken;

            if (status == "Exception")
                return BuildState.Broken;

            return BuildState.NotConnected;
        }
    }
}
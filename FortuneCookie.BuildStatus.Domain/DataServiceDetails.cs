using System;

namespace FortuneCookie.BuildStatus.Domain
{
    /// <summary>
    /// Class describing the service endpoint where data can be obtained from
    /// </summary>
    public class DataServiceDetails
    {
        public DataServiceDetails(string name, string dataAddress)
        {
            DataAddress = dataAddress;
            DashboardAddress = dataAddress;
            Name = name;
            TimeZoneOffset = new TimeSpan(0);
        }

        public DataServiceDetails(string name, string dataAddress, string dashBoardAddress)
        {
            DataAddress = dataAddress;
            DashboardAddress = dashBoardAddress;
            Name = name;
            TimeZoneOffset = new TimeSpan(0);
        }

        public DataServiceDetails(string name, string dataAddress, string dashBoardAddress, TimeSpan timeZoneOffset)
        {
            DataAddress = dataAddress;
            DashboardAddress = dashBoardAddress;
            Name = name;
            TimeZoneOffset = timeZoneOffset;
        }

        public string DataAddress { get; set; }
        public string DashboardAddress { get; set; }
        public string Name { get; set; }
        public TimeSpan TimeZoneOffset { get; set; }
    }
}
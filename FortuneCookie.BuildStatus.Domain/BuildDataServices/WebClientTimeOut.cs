using System;
using System.Net;

namespace FortuneCookie.BuildStatus.Domain.BuildDataServices
{
    public class WebClientTimeOut : WebClient
    {
        public WebClientTimeOut(int timeout)
        {
            Timeout = timeout;
        }

        protected int Timeout { get; set; }
        
        public WebClientTimeOut()
        {
            Timeout = 5*1000;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest webRequest = base.GetWebRequest(uri);
            webRequest.Timeout = Timeout;
            return webRequest;
        }
    }
}
using System;
using System.Web.Mvc;
using FortuneCookie.BuildStatus.Domain.BuildDataServices;
using FortuneCookie.BuildStatus.Domain.BuildStatusProviders;
using FortuneCookie.BuildStatus.Web.Controllers;
using NUnit.Framework;

namespace FortuneCookie.BuildStatus.Test.Web
{
    [TestFixture]
    public class HomeController_Test
    {
        [SetUp]
        public void InitTest()
        {
            var statusProvider = new BuildStatusProvider(new MockDataService());
            Controller = new HomeController(statusProvider);
        }

        protected HomeController Controller;

        
        [Test]
        public void DefaultActionUsesConventionToChooseView()
        {
            var result = Controller.Index() as ViewResult;
            Assert.IsTrue(String.IsNullOrEmpty(result.ViewName));
        }
    }
}
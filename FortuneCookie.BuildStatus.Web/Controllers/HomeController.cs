using System.Web.Mvc;
using FortuneCookie.BuildStatus.Domain.BuildStatusProviders;
using FortuneCookie.BuildStatus.Web.Models;

namespace FortuneCookie.BuildStatus.Web.Controllers
{
    public class HomeController : Controller
    {
        protected IBuildStatusProvider BuildStatusProvider { get; set; }

        public HomeController(IBuildStatusProvider buildStatusProvider)
        {
            BuildStatusProvider = buildStatusProvider;
        }

        public ActionResult Index()
        {
            var model = new BuildStatusViewModel(BuildStatusProvider);
            return View(model);
        }

        public ActionResult CurrentStatusFragment()
        {
            var model = new BuildStatusViewModel(BuildStatusProvider);
            return PartialView(model.CurrentBuildStatus, model);
        }

        public ActionResult Summary()
        {
            var model = new BuildDetailsViewModel(BuildStatusProvider);
            return PartialView("Summary", model);
        }

    }
}

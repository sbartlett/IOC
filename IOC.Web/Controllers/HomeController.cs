using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IOC.Web.QuoteProvider;

namespace IOC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteProvider _quoteProvider;

        public HomeController(IQuoteProvider quoteProvider)
        {
            _quoteProvider = quoteProvider;
        }

        public ActionResult Index()
        {
            var quote = _quoteProvider.GetQuote();

            return View(new HomeModel { Quote = quote});
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class HomeModel
    {
        public string Quote { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class TagsController : Controller
    {
        // GET: Tags
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Tags.ToList());
        }
    }
}
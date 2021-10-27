using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Models;
using PagedList;

namespace StackOverflow.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.HotSortParm = sortOrder == "hot_desc" ? "" : "hot_desc";

            var questions = from q in db.Questions
                            select q;

            switch (sortOrder)
            {
                case "hot_desc":
                    questions = questions.OrderByDescending(q => q.Answers.Count);
                    break;
                case "date":
                    questions = questions.OrderBy(q => q.RelativeTime);
                    break;
                case "date_desc":
                    questions = questions.OrderByDescending(q => q.RelativeTime);
                    break;
                default:
                    questions = questions.OrderByDescending(q => q.RelativeTime);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, pageSize));
        }
    }
}
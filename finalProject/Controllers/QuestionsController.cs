 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StackOverflow.Models;
using PagedList;
using Humanizer;
using System.Threading;
using System.Data.Entity.Validation;

namespace StackOverflow.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Questions
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

        public ActionResult Details(int? id, string title)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            ViewBag.DateSubstract = question.RelativeTime.Humanize();
            ViewData["user"] = User.Identity.GetUserId();
            return View(question);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Details(int? id, string qcomment, int? answerId, string acomment, string answer)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Question question = db.Questions.Find(id);

            if (question == null)
            {
                return HttpNotFound();
            }

            if (qcomment != null && qcomment != string.Empty)
            {
                QuestionComment questionComment = new QuestionComment();
                questionComment.UserId = User.Identity.GetUserId();
                questionComment.RelativeTime = DateTime.Now;
                questionComment.QuestionId = question.Id;
                questionComment.Content = qcomment;

                db.Comments.Add(questionComment);
                db.SaveChanges();
            }

            if (acomment != null && acomment != string.Empty && answerId != null)
            {
                Answer aswr = db.Answers.Find(answerId);
                if (aswr == null)
                {
                    return HttpNotFound();
                }
                AnswerComment answerComment = new AnswerComment();
                answerComment.UserId = User.Identity.GetUserId();
                answerComment.RelativeTime = DateTime.Now;
                answerComment.AnswerId = aswr.Id;
                answerComment.Content = acomment;

                db.Comments.Add(answerComment);
                db.SaveChanges();
            }

            if (answer != null && answer != string.Empty)
            {
                Answer aswr = new Answer();
                aswr.RelativeTime = DateTime.Now;
                aswr.Content = answer;
                aswr.UserId = User.Identity.GetUserId();
                aswr.QuestionId = question.Id;

                db.Answers.Add(aswr);
                db.SaveChanges();
            }

            ViewBag.DateSubstract = question.RelativeTime.Humanize();
            return RedirectToAction("Details", "Questions", new { id, title = question.Title });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AcceptAnswer(int? qId, int? aId)
        {
            if (qId == null || aId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Answer answer = db.Answers.Find(aId);

            if (answer == null)
            {
                return HttpNotFound();
            }

            answer.IsAcceptedAnswer = answer.IsAcceptedAnswer ? false : true;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = qId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Vote(int? qId, int? aId, bool isUp)
        {
            if (qId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(User.Identity.GetUserId());

            Question question = db.Questions.Find(qId);
            Answer answer = db.Answers.Find(aId);

            if (answer != null)
            {
                if (user.Id == answer.User?.Id)
                {
                    TempData["answerVoteError"] = "error";
                    Thread.Sleep(4000);
                    return RedirectToAction("Details", new { id = qId });
                }
                var vote = answer.Votes.SingleOrDefault(v => v.UserId == user.Id);

                if (vote != null)
                {
                    if (vote.VoteType == VoteType.Up)
                    {
                        vote.VoteType = isUp ? VoteType.None : VoteType.Down;
                        answer.User.Reputation += isUp ? -5 : -10;
                    }
                    else if (vote.VoteType == VoteType.Down)
                    {
                        vote.VoteType = isUp ? VoteType.Up : VoteType.None;
                        answer.User.Reputation += isUp ? 10 : 5;
                    }
                    else
                    {
                        vote.VoteType = isUp ? VoteType.Up : VoteType.Down;
                        answer.User.Reputation += isUp ? 5 : -5;
                    }
                }
                else
                {
                    AnswerVote answerVote = new AnswerVote();
                    answerVote.UserId = user.Id;
                    answerVote.AnswerId = answer.Id;
                    answerVote.VoteType = isUp ? VoteType.Up : VoteType.Down;
                    answer.User.Reputation += isUp ? 5 : -5;

                    db.Votes.Add(answerVote);
                }
                db.SaveChanges();
            }

            if (question != null)
            {
                if (user.Id == question.User.Id)
                {
                    TempData["questionVoteError"] = "error";
                    Thread.Sleep(4000);
                    return RedirectToAction("Details", new { id = qId });
                }

                var vote = question.Votes.SingleOrDefault(v => v.UserId == user.Id);

                if (vote != null)
                {
                    if (vote.VoteType == VoteType.Up)
                    {
                        vote.VoteType = isUp ? VoteType.None : VoteType.Down;
                        question.User.Reputation += isUp ? -5 : -10;
                    }
                    else if (vote.VoteType == VoteType.Down)
                    {
                        vote.VoteType = isUp ? VoteType.Up : VoteType.None;
                        question.User.Reputation += isUp ? 10 : 5;
                    }
                    else
                    {
                        vote.VoteType = isUp ? VoteType.Up : VoteType.Down;
                        question.User.Reputation += isUp ? 5 : -5;
                    }
                }
                else
                {
                    QuestionVote questionVote = new QuestionVote();
                    questionVote.UserId = user.Id;
                    questionVote.QuestionId = question.Id;
                    questionVote.VoteType = isUp ? VoteType.Up : VoteType.Down;
                    question.User.Reputation += isUp ? 5 : -5;

                    db.Votes.Add(questionVote);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = qId });
        }

        [Authorize]
        public ActionResult Ask()
        {
            AskQuestionViewModel viewModel = new AskQuestionViewModel();
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Ask([Bind(Include = "Title, Description")] AskQuestionViewModel viewModel)
        {
            var tagForm = Request.Form["tags"];

            var tags = Regex.Replace(tagForm, @"[^a-zA-Z][\bvalue\b]", " ").Split(',').ToList();

            if (tagForm == "")
            {
                TempData["tagError"] = "error";
                Thread.Sleep(4000);

                return View(viewModel);
            }

            Question question = new Question();
            if (ModelState.IsValid)
            {
                question.UserId = User.Identity.GetUserId();
                question.RelativeTime = DateTime.Now;
                question.Title = viewModel.Title;
                question.Description = viewModel.Description;

                db.Questions.Add(question);

                foreach (var tag in tags)
                {
                    QuestionTag qt = new QuestionTag();

                    Tag t = db.Tags.FirstOrDefault(tt => tt.Name == tag);

                    if (t == null)
                    {
                        t = new Tag { Name = tag };
                        db.Tags.Add(t);
                    }

                    qt.QuestionId = question.Id;
                    qt.TagId = t.Id;
                    db.QuestionTags.Add(qt);
                }
            }
            try
            {
                db.SaveChanges();
                
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Tagged(string id, string sortOrder, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tag tag = db.Tags.FirstOrDefault(t => t.Name == id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            ViewBag.TagName = tag.Name;
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.HotSortParm = sortOrder == "hot_desc" ? "" : "hot_desc";

            var questions = from q in tag.Questions
                            select q.Question;

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
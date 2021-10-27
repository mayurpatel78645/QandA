using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public DateTime RelativeTime { get; set; }
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public int VoteScore { get => Votes.Sum(v => (int)v.VoteType); }
        public bool IsAcceptedAnswer { get; set; } = false;
        public virtual ApplicationUser User { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<AnswerComment> Comments { get; set; }
        public virtual ICollection<AnswerVote> Votes { get; set; }
        public Answer()
        {
            this.Comments = new HashSet<AnswerComment>();
            this.Votes = new HashSet<AnswerVote>();
        }
    }
}
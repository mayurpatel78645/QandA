using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflow.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MinLength(10, ErrorMessage = "Title must be at least 10 characters")]
        public string Title { get; set; }
        [AllowHtml]
        [Display(Name="Body")]
        [DataType(DataType.Html)]
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime RelativeTime { get; set; }
        public int VoteScore { get => Votes.Sum(v => (int)v.VoteType); }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<QuestionTag> Tags { get; set; }
        public virtual ICollection<QuestionComment> Comments { get; set; }
        public virtual ICollection<QuestionVote> Votes { get; set; }
        public Question()
        {
            this.Answers = new HashSet<Answer>();
            this.Comments = new HashSet<QuestionComment>();
            this.Tags = new HashSet<QuestionTag>();
            this.Votes = new HashSet<QuestionVote>();
        }
    }
}
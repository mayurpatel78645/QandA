using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StackOverflow.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime RelativeTime { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    
    public class AnswerComment : Comment
    {
        public int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
    }

    public class QuestionComment : Comment
    {
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
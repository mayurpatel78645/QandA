using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflow.Models
{
    public enum VoteType
    {
        None = 0,
        Up = 1,
        Down = -1
    }
    public class Vote
    {
        public int Id { get; set; }
        public VoteType VoteType { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public class QuestionVote : Vote
    {
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }

    public class AnswerVote : Vote
    {
        public int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverflow.Models
{
    public class AskQuestionViewModel
    {
        public string Title { get; set; }
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Description { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
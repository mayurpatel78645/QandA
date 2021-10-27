using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace StackOverflow.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string Extract { get { return GetExtract(); }}
        public virtual ICollection<QuestionTag> Questions { get; set; }

        public Tag()
        {
            this.Questions = new HashSet<QuestionTag>();
        }

        public string GetExtract()
        {
            WebRequest request = WebRequest.Create($"https://en.wikipedia.org/api/rest_v1/page/summary/{this.Name}");
            WebResponse response = request.GetResponse();

            string extract = "";

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JObject json = JObject.Parse(responseFromServer);
                extract = json["extract"].ToString();
            }
            response.Close();

            return extract;
        }
    }
}
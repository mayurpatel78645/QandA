namespace StackOverflow.Migrations
{
    using StackOverflow.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StackOverflow.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StackOverflow.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var questions = new List<Question>
            {
            new Question { Description = "this is a test seed method",     Title = "title seed"},
            };
            questions.ForEach(s => context.Questions.AddOrUpdate(i => i.Title, s));
            context.SaveChanges();

            var departments = new List<Tag>
            {
            new Tag { Name = "Java"},
            };
            departments.ForEach(s => context.Tags.AddOrUpdate(d => d.Name, s));
            context.SaveChanges();
            
        }
    }
}

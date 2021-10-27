namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        RelativeTime = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        AnswerId = c.Int(),
                        QuestionId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.AnswerId, cascadeDelete: false)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AnswerId)
                .Index(t => t.QuestionId);
            
            AddColumn("dbo.Answers", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Answers", "UserId");
            AddForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Comments", "AnswerId", "dbo.Answers");
            DropIndex("dbo.Comments", new[] { "QuestionId" });
            DropIndex("dbo.Comments", new[] { "AnswerId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropColumn("dbo.Answers", "UserId");
            DropTable("dbo.Comments");
        }
    }
}

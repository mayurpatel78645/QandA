namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVotes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VoteType = c.Int(nullable: false),
                        QuestionId = c.Int(),
                        AnswerId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: false)
                .ForeignKey("dbo.Answers", t => t.AnswerId, cascadeDelete: false)
                .Index(t => t.QuestionId)
                .Index(t => t.AnswerId);  
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "AnswerId", "dbo.Answers");
            DropForeignKey("dbo.Votes", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Votes", new[] { "AnswerId" });
            DropIndex("dbo.Votes", new[] { "QuestionId" });
            DropTable("dbo.Votes");
        }
    }
}

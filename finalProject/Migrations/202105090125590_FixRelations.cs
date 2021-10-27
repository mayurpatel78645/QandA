namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRelations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "VoteScore", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "VoteScore", c => c.Int(nullable: false));
            DropColumn("dbo.Votes", "VoteScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "VoteScore", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "VoteScore");
            DropColumn("dbo.Answers", "VoteScore");
        }
    }
}

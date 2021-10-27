namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVoteScoreGetter : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Answers", "VoteScore");
            DropColumn("dbo.Questions", "VoteScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "VoteScore", c => c.Int(nullable: false));
            AddColumn("dbo.Answers", "VoteScore", c => c.Int(nullable: false));
        }
    }
}

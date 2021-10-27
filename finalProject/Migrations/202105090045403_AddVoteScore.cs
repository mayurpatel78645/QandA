namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVoteScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "VoteScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "VoteScore");
        }
    }
}

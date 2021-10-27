namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccepttedAnswers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "IsAcceptedAnswer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "IsAcceptedAnswer");
        }
    }
}

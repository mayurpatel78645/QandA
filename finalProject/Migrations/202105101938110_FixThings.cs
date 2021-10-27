namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixThings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Description", c => c.String(nullable: false));
        }
    }
}

namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMaxlengthOfTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Title", c => c.String(nullable: false, maxLength: 35));
        }
    }
}

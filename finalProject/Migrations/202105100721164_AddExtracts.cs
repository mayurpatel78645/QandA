namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtracts : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tags", "Extract");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Extract", c => c.String());
        }
    }
}

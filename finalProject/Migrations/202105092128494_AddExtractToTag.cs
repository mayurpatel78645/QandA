namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtractToTag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Extract", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "Extract");
        }
    }
}

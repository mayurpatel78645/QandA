namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Questions", new[] { "User_Id" });
            DropColumn("dbo.Questions", "UserId");
            RenameColumn(table: "dbo.Questions", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Questions", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Questions", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Questions", new[] { "UserId" });
            AlterColumn("dbo.Questions", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Questions", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Questions", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "User_Id");
        }
    }
}

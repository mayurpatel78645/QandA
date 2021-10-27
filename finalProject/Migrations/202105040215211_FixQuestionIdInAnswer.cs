namespace StackOverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixQuestionIdInAnswer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropColumn("dbo.Answers", "QuestionId");
            RenameColumn(table: "dbo.Answers", name: "Question_Id", newName: "QuestionId");
            AddColumn("dbo.Answers", "Content", c => c.String());
            AlterColumn("dbo.Answers", "QuestionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Answers", "QuestionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Answers", "QuestionId");
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            AlterColumn("dbo.Answers", "QuestionId", c => c.Int());
            AlterColumn("dbo.Answers", "QuestionId", c => c.String());
            DropColumn("dbo.Answers", "Content");
            RenameColumn(table: "dbo.Answers", name: "QuestionId", newName: "Question_Id");
            AddColumn("dbo.Answers", "QuestionId", c => c.String());
            CreateIndex("dbo.Answers", "Question_Id");
            AddForeignKey("dbo.Answers", "Question_Id", "dbo.Questions", "Id");
        }
    }
}

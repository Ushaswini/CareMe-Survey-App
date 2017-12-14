namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterQuestionTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Minimum", c => c.Int(nullable: false));
            AlterColumn("dbo.Questions", "Maximum", c => c.Int(nullable: false));
            AlterColumn("dbo.Questions", "StepSize", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "StepSize", c => c.Double(nullable: false));
            AlterColumn("dbo.Questions", "Maximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Questions", "Minimum", c => c.Double(nullable: false));
        }
    }
}

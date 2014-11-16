namespace SimpleTasks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "PriorityLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "PriorityLevel");
        }
    }
}

namespace SimpleTasks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "IsDeleted");
        }
    }
}

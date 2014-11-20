namespace SimpleTasks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Owner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Owner");
        }
    }
}

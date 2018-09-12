namespace Project1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateGenreTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT into Genres (Name) VALUES ('Comedy')");
            Sql("INSERT into Genres (Name) VALUES ('Action')");
            Sql("INSERT into Genres (Name) VALUES ('Romance')");
            Sql("INSERT into Genres (Name) VALUES ('Family')");

        }

        public override void Down()
        {
        }
    }
}

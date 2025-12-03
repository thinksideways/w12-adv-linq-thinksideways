using ConsoleRpgEntities.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class InitialSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Get the name of the migration class
            string migrationName = GetType().Name;

            // Get the SQL script content
            string sql = MigrationHelper.GetMigrationScript(migrationName, "Up");

            // Execute the SQL script
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Get the name of the migration class
            string migrationName = GetType().Name;

            // Get the rollback SQL script content
            string sql = MigrationHelper.GetMigrationScript(migrationName, "Down");

            // Execute the SQL script
            migrationBuilder.Sql(sql);
        }
    }
}

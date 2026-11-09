using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstimatorX.Migrations.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectAssumptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Assumptions",
                table: "Project",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assumptions",
                table: "Project");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Report.Infrastructure.Migrations
{
    public partial class RemoveQuestionNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionName",
                schema: "report",
                table: "questionUnits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuestionName",
                schema: "report",
                table: "questionUnits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

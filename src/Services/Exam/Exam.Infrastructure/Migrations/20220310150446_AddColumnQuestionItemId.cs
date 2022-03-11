using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Exam.Infrastructure.Migrations
{
    public partial class AddColumnQuestionItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExam",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "QuestionItemId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionItemId",
                table: "Questions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateExam",
                table: "Exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

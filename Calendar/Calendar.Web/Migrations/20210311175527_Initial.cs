using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Calendar.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoTasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoTasks", x => x.TaskId);
                });

            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "TaskId", "DueDate", "Text" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2021, 3, 13, 14, 0, 2, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Clean house" });

            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "TaskId", "DueDate", "Text" },
                values: new object[] { 2, new DateTimeOffset(new DateTime(2021, 3, 14, 14, 30, 2, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Deploy project" });

            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "TaskId", "DueDate", "Text" },
                values: new object[] { 3, new DateTimeOffset(new DateTime(2021, 3, 13, 15, 30, 2, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Do some management" });

            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "TaskId", "DueDate", "Text" },
                values: new object[] { 4, new DateTimeOffset(new DateTime(2021, 3, 14, 12, 0, 2, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Go shopping" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoTasks");
        }
    }
}

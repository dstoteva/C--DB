using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class TimeSpanFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}

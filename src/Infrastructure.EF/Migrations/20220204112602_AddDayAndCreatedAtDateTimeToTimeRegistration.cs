using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.EF.Migrations
{
    public partial class AddDayAndCreatedAtDateTimeToTimeRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "TimeRegistrations",
                newName: "Day");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TimeRegistrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TimeRegistrations");

            migrationBuilder.RenameColumn(
                name: "Day",
                table: "TimeRegistrations",
                newName: "DateTime");
        }
    }
}

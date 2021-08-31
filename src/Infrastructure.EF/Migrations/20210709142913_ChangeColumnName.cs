using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.EF.Migrations
{
    public partial class ChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Workers",
                newName: "WorkerId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TimeRegistrations",
                newName: "TimeRegistrationId");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Orders",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Orders",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Expenses",
                newName: "ExpenseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Workers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TimeRegistrationId",
                table: "TimeRegistrations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "OrderStatus");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Orders",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Orders",
                newName: "End");

            migrationBuilder.RenameColumn(
                name: "ExpenseId",
                table: "Expenses",
                newName: "Id");
        }
    }
}

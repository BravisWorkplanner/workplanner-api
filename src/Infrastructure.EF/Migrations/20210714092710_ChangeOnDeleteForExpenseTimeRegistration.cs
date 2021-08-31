using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.EF.Migrations
{
    public partial class ChangeOnDeleteForExpenseTimeRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Orders_OrderId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeRegistrations_Orders_OrderId",
                table: "TimeRegistrations");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Orders_OrderId",
                table: "Expenses",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRegistrations_Orders_OrderId",
                table: "TimeRegistrations",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Orders_OrderId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeRegistrations_Orders_OrderId",
                table: "TimeRegistrations");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Orders_OrderId",
                table: "Expenses",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRegistrations_Orders_OrderId",
                table: "TimeRegistrations",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

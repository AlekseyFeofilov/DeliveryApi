using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishBaskets_Orders_OrderId",
                table: "DishBaskets");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "DishBaskets",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "DishBaskets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DishBaskets_UserId",
                table: "DishBaskets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishBaskets_Orders_OrderId",
                table: "DishBaskets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DishBaskets_Users_UserId",
                table: "DishBaskets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishBaskets_Orders_OrderId",
                table: "DishBaskets");

            migrationBuilder.DropForeignKey(
                name: "FK_DishBaskets_Users_UserId",
                table: "DishBaskets");

            migrationBuilder.DropIndex(
                name: "IX_DishBaskets_UserId",
                table: "DishBaskets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DishBaskets");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "DishBaskets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DishBaskets_Orders_OrderId",
                table: "DishBaskets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

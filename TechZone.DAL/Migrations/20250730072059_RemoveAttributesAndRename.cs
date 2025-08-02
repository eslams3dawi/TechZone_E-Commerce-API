using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechZone.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAttributesAndRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_OrderHeaders_OrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Payments",
                newName: "OrderHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                newName: "IX_Payments_OrderHeaderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippingDate",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_OrderHeaders_OrderHeaderId",
                table: "Payments",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_OrderHeaders_OrderHeaderId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "OrderHeaderId",
                table: "Payments",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_OrderHeaderId",
                table: "Payments",
                newName: "IX_Payments_OrderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippingDate",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "OrderHeaders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "OrderHeaders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_OrderHeaders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

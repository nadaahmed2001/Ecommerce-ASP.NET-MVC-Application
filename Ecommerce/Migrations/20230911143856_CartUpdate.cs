using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class CartUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_ProdID",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Carts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ProdID",
                table: "Carts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ProdID",
                table: "Carts",
                newName: "IX_Carts_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Carts",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Carts",
                newName: "ProdID");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                newName: "IX_Carts_ProdID");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Carts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_ProdID",
                table: "Carts",
                column: "ProdID",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}

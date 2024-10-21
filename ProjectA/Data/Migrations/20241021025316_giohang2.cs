using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectA.Data.Migrations
{
    /// <inheritdoc />
    public partial class giohang2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SanPham",
                table: "GioHang");

            migrationBuilder.RenameColumn(
                name: "Quanlity",
                table: "GioHang",
                newName: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "SanPhamId",
                table: "GioHang",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_SanPhamId",
                table: "GioHang",
                column: "SanPhamId");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_SanPham_SanPhamId",
                table: "GioHang",
                column: "SanPhamId",
                principalTable: "SanPham",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_SanPham_SanPhamId",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_SanPhamId",
                table: "GioHang");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "GioHang",
                newName: "Quanlity");

            migrationBuilder.AlterColumn<string>(
                name: "SanPhamId",
                table: "GioHang",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SanPham",
                table: "GioHang",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

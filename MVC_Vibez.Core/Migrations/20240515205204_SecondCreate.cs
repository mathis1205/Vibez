using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Vibez.Core.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LoginToken_LoginTokenId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "LoginTokenId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LoginToken_LoginTokenId",
                table: "Users",
                column: "LoginTokenId",
                principalTable: "LoginToken",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LoginToken_LoginTokenId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "LoginTokenId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LoginToken_LoginTokenId",
                table: "Users",
                column: "LoginTokenId",
                principalTable: "LoginToken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

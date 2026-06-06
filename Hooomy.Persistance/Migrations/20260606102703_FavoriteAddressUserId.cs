using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class FavoriteAddressUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "FavoriteAddresses",
                newName: "UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FavoriteAddresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FavoriteAddresses",
                newName: "UserID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FavoriteAddresses",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}

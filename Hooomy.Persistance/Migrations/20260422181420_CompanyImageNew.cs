using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class CompanyImageNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyImages_IsMain",
                table: "CompanyImages");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "CompanyImages");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "CompanyImages");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "CompanyImages",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "CompanyImages",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                table: "CompanyImages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "CompanyImages");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "CompanyImages");

            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "CompanyImages");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "CompanyImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "CompanyImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyImages_IsMain",
                table: "CompanyImages",
                column: "IsMain");
        }
    }
}

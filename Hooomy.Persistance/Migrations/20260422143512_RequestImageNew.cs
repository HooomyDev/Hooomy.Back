using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RequestImageNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequestImages_IsMain",
                table: "RequestImages");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "RequestImages");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "RequestImages");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "RequestImages",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "RequestImages",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                table: "RequestImages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "RequestImages");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "RequestImages");

            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "RequestImages");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "RequestImages",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "RequestImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_RequestImages_IsMain",
                table: "RequestImages",
                column: "IsMain");
        }
    }
}

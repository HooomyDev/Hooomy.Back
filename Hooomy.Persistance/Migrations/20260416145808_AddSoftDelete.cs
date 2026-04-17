using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "RequestImages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RequestImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "RequestComments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RequestComments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "RequestImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RequestImages");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "RequestComments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RequestComments");
        }
    }
}

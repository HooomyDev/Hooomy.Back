using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RequestCommentImage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "RequestComments");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "RequestComments");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "RequestComments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_CompanyId",
                table: "RequestComments",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_Companies_CompanyId",
                table: "RequestComments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_Companies_CompanyId",
                table: "RequestComments");

            migrationBuilder.DropIndex(
                name: "IX_RequestComments_CompanyId",
                table: "RequestComments");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "RequestComments");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "RequestComments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "RequestComments",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }
    }
}

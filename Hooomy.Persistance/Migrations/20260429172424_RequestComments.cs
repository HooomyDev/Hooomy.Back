using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RequestComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestId1",
                table: "RequestComments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_RequestId1",
                table: "RequestComments",
                column: "RequestId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_Requests_RequestId1",
                table: "RequestComments",
                column: "RequestId1",
                principalTable: "Requests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_Requests_RequestId1",
                table: "RequestComments");

            migrationBuilder.DropIndex(
                name: "IX_RequestComments_RequestId1",
                table: "RequestComments");

            migrationBuilder.DropColumn(
                name: "RequestId1",
                table: "RequestComments");
        }
    }
}

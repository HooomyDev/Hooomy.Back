using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class CompanyAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Addresses_AddressId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_AddressId",
                table: "Companies");

            migrationBuilder.AddColumn<Guid>(
                name: "RegisteredCompanyId",
                table: "Addresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServicedByCompanyId",
                table: "Addresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RegisteredCompanyId",
                table: "Addresses",
                column: "RegisteredCompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ServicedByCompanyId",
                table: "Addresses",
                column: "ServicedByCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Companies_RegisteredCompanyId",
                table: "Addresses",
                column: "RegisteredCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Companies_ServicedByCompanyId",
                table: "Addresses",
                column: "ServicedByCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Companies_RegisteredCompanyId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Companies_ServicedByCompanyId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_RegisteredCompanyId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ServicedByCompanyId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "RegisteredCompanyId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ServicedByCompanyId",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Addresses_AddressId",
                table: "Companies",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

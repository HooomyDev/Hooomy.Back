using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class PollTypeAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Polls_IsActive",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Polls");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Polls",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Polls");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Polls",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_IsActive",
                table: "Polls",
                column: "IsActive");
        }
    }
}

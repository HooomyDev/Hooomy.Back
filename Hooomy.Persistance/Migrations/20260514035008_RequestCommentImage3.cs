using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RequestCommentImage3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RequestComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RequestComments");
        }
    }
}

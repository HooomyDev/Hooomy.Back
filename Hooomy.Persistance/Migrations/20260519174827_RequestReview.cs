using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RequestReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Requests",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "RequestReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestReviews_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestReviews_RequestId",
                table: "RequestReviews",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestReviews_Score",
                table: "RequestReviews",
                column: "Score");

            migrationBuilder.CreateIndex(
                name: "IX_RequestReviews_UserId",
                table: "RequestReviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestReviews");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Requests",
                newName: "UserID");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Requests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

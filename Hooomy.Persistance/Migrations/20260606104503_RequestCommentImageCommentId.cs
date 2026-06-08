using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hooome.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RequestCommentImageCommentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestCommentsImages_RequestComments_RequestCommentId",
                table: "RequestCommentsImages");

            migrationBuilder.RenameColumn(
                name: "RequestCommentId",
                table: "RequestCommentsImages",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestCommentsImages_RequestCommentId",
                table: "RequestCommentsImages",
                newName: "IX_RequestCommentsImages_CommentId");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "Requests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestCommentsImages_RequestComments_CommentId",
                table: "RequestCommentsImages",
                column: "CommentId",
                principalTable: "RequestComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestCommentsImages_RequestComments_CommentId",
                table: "RequestCommentsImages");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "RequestCommentsImages",
                newName: "RequestCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestCommentsImages_CommentId",
                table: "RequestCommentsImages",
                newName: "IX_RequestCommentsImages_RequestCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestCommentsImages_RequestComments_RequestCommentId",
                table: "RequestCommentsImages",
                column: "RequestCommentId",
                principalTable: "RequestComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

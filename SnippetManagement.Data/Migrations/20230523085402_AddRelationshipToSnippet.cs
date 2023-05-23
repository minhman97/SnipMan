using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnippetManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipToSnippet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Snippet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("3b094ed2-5eb3-4b6e-150c-08db05b7cf56"));

            migrationBuilder.CreateIndex(
                name: "IX_Snippet_UserId",
                table: "Snippet",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Snippet_User_UserId",
                table: "Snippet",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snippet_User_UserId",
                table: "Snippet");

            migrationBuilder.DropIndex(
                name: "IX_Snippet_UserId",
                table: "Snippet");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Snippet");
        }
    }
}

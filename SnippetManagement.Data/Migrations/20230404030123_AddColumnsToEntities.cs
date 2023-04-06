using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnippetManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "User",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Modified",
                table: "User",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "Tag",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Modified",
                table: "Tag",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Tag");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnippetManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageToSnippet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "User",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "Tag",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Snippet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Snippet");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "User",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "Tag",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "getutcdate()");
        }
    }
}

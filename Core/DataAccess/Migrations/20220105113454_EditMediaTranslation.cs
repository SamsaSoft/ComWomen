using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DataAccess.Migrations
{
    public partial class EditMediaTranslation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaTranslations_AspNetUsers_EditorId",
                table: "MediaTranslations");

            migrationBuilder.DropIndex(
                name: "IX_MediaTranslations_EditorId",
                table: "MediaTranslations");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "MediaTranslations");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "MediaTranslations");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "MediaTranslations",
                newName: "Language");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "MediaTranslations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Language",
                table: "MediaTranslations",
                newName: "LanguageId");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "MediaTranslations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "MediaTranslations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditorId",
                table: "MediaTranslations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaTranslations_EditorId",
                table: "MediaTranslations",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaTranslations_AspNetUsers_EditorId",
                table: "MediaTranslations",
                column: "EditorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

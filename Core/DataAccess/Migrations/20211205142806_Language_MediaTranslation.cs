using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.DataAccess.Migrations
{
    public partial class Language_MediaTranslation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MediaId = table.Column<int>(type: "integer", nullable: true),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    EditedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EditorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaTranslation_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaTranslation_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaTranslation_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "IsEnabled", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { 1, true, "en", "English" },
                    { 2, true, "ru", "русский" },
                    { 3, true, "ky", "кыргызча" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaTranslation_AuthorId",
                table: "MediaTranslation",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaTranslation_EditorId",
                table: "MediaTranslation",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaTranslation_LanguageId",
                table: "MediaTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaTranslation_MediaId",
                table: "MediaTranslation",
                column: "MediaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaTranslation");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}

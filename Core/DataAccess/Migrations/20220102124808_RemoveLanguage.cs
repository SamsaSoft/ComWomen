using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DataAccess.Migrations
{
    public partial class RemoveLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaTranslations_Languages_LanguageId",
                table: "MediaTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaTranslations_Medias_MediaId",
                table: "MediaTranslations");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_MediaTranslations_LanguageId",
                table: "MediaTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MediaTranslations",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "LanguageId",
                table: "MediaTranslations",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaTranslations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaTranslations_Medias_MediaId",
                table: "MediaTranslations",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaTranslations_Medias_MediaId",
                table: "MediaTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MediaTranslations",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "MediaTranslations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaTranslations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "IsEnabled", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { 1, true, "en", "English" },
                    { 2, true, "ru", "Русский" },
                    { 3, true, "ky", "Кыргызча" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaTranslations_LanguageId",
                table: "MediaTranslations",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaTranslations_Languages_LanguageId",
                table: "MediaTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaTranslations_Medias_MediaId",
                table: "MediaTranslations",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id");
        }
    }
}

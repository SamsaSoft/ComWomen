using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DataAccess.Migrations
{
    public partial class RemoveMediaType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_MediaTypes_MediaTypeId",
                table: "Medias");

            migrationBuilder.DropTable(
                name: "MediaTypes");

            migrationBuilder.DropIndex(
                name: "IX_Medias_MediaTypeId",
                table: "Medias");

            migrationBuilder.RenameColumn(
                name: "MediaTypeId",
                table: "Medias",
                newName: "MediaType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "Medias",
                newName: "MediaTypeId");

            migrationBuilder.CreateTable(
                name: "MediaTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MediaTypes",
                columns: new[] { "Id", "IsEnabled", "Name" },
                values: new object[,]
                {
                    { 1, true, "Photo" },
                    { 2, true, "Video" },
                    { 3, true, "Audio" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_MediaTypeId",
                table: "Medias",
                column: "MediaTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_MediaTypes_MediaTypeId",
                table: "Medias",
                column: "MediaTypeId",
                principalTable: "MediaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

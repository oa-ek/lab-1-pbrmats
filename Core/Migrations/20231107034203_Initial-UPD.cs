using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBRmats.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialUPD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialsCollections_AppUser_AppUserId",
                table: "MaterialsCollections");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialsCollections_AppUser_AppUserId",
                table: "MaterialsCollections",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialsCollections_AppUser_AppUserId",
                table: "MaterialsCollections");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialsCollections_AppUser_AppUserId",
                table: "MaterialsCollections",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

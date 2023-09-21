using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBRmats.Core.Migrations
{
    /// <inheritdoc />
    public partial class MaterialsCollectionsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterialsCollections_MaterialsCollectionId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_MaterialsCollectionId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "MaterialsCollectionId",
                table: "Materials");

            migrationBuilder.CreateTable(
                name: "MaterialMaterialsCollection",
                columns: table => new
                {
                    MaterialsCollectionId = table.Column<int>(type: "int", nullable: false),
                    MaterialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMaterialsCollection", x => new { x.MaterialsCollectionId, x.MaterialsId });
                    table.ForeignKey(
                        name: "FK_MaterialMaterialsCollection_MaterialsCollections_MaterialsCollectionId",
                        column: x => x.MaterialsCollectionId,
                        principalTable: "MaterialsCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialMaterialsCollection_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMaterialsCollection_MaterialsId",
                table: "MaterialMaterialsCollection",
                column: "MaterialsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialMaterialsCollection");

            migrationBuilder.AddColumn<int>(
                name: "MaterialsCollectionId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialsCollectionId",
                table: "Materials",
                column: "MaterialsCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterialsCollections_MaterialsCollectionId",
                table: "Materials",
                column: "MaterialsCollectionId",
                principalTable: "MaterialsCollections",
                principalColumn: "Id");
        }
    }
}

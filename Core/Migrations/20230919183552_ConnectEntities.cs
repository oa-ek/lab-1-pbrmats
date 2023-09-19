using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBRmats.Core.Migrations
{
    /// <inheritdoc />
    public partial class ConnectEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_License_Materials_MaterialId",
                table: "License");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Category_CategoryId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Source_Materials_MaterialId",
                table: "Source");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Source",
                table: "Source");

            migrationBuilder.DropIndex(
                name: "IX_Source_MaterialId",
                table: "Source");

            migrationBuilder.DropPrimaryKey(
                name: "PK_License",
                table: "License");

            migrationBuilder.DropIndex(
                name: "IX_License_MaterialId",
                table: "License");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Source");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "License");

            migrationBuilder.RenameTable(
                name: "Source",
                newName: "Sources");

            migrationBuilder.RenameTable(
                name: "License",
                newName: "Licenses");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "MaterialsCollectionId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sources",
                table: "Sources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Licenses",
                table: "Licenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MaterialLicenses",
                columns: table => new
                {
                    LicensesId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLicenses", x => new { x.LicensesId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_MaterialLicenses_Licenses_LicensesId",
                        column: x => x.LicensesId,
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialLicenses_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialSources",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    SourcesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSources", x => new { x.MaterialId, x.SourcesId });
                    table.ForeignKey(
                        name: "FK_MaterialSources_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSources_Sources_SourcesId",
                        column: x => x.SourcesId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialsCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialsCollections_Users_ParentUserId",
                        column: x => x.ParentUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialsCollectionId",
                table: "Materials",
                column: "MaterialsCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLicenses_MaterialId",
                table: "MaterialLicenses",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsCollections_ParentUserId",
                table: "MaterialsCollections",
                column: "ParentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSources_SourcesId",
                table: "MaterialSources",
                column: "SourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Categories_CategoryId",
                table: "Materials",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_MaterialsCollections_MaterialsCollectionId",
                table: "Materials",
                column: "MaterialsCollectionId",
                principalTable: "MaterialsCollections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Categories_CategoryId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_MaterialsCollections_MaterialsCollectionId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "MaterialLicenses");

            migrationBuilder.DropTable(
                name: "MaterialsCollections");

            migrationBuilder.DropTable(
                name: "MaterialSources");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Materials_MaterialsCollectionId",
                table: "Materials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sources",
                table: "Sources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Licenses",
                table: "Licenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "MaterialsCollectionId",
                table: "Materials");

            migrationBuilder.RenameTable(
                name: "Sources",
                newName: "Source");

            migrationBuilder.RenameTable(
                name: "Licenses",
                newName: "License");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Source",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "License",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Source",
                table: "Source",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_License",
                table: "License",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Source_MaterialId",
                table: "Source",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_License_MaterialId",
                table: "License",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_License_Materials_MaterialId",
                table: "License",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Category_CategoryId",
                table: "Materials",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Source_Materials_MaterialId",
                table: "Source",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");
        }
    }
}

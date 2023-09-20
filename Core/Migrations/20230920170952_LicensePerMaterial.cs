using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBRmats.Core.Migrations
{
    /// <inheritdoc />
    public partial class LicensePerMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialLicenses");

            migrationBuilder.AddColumn<int>(
                name: "LicenseId",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_LicenseId",
                table: "Materials",
                column: "LicenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Licenses_LicenseId",
                table: "Materials",
                column: "LicenseId",
                principalTable: "Licenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Licenses_LicenseId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_LicenseId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "LicenseId",
                table: "Materials");

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

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLicenses_MaterialId",
                table: "MaterialLicenses",
                column: "MaterialId");
        }
    }
}

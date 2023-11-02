using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBRmats.Core.Migrations
{
    /// <inheritdoc />
    public partial class MoreDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AvgIOR",
                table: "Materials",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AvgMetallic",
                table: "Materials",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "AvgSpecularColor",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgIOR",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AvgMetallic",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AvgSpecularColor",
                table: "Materials");
        }
    }
}

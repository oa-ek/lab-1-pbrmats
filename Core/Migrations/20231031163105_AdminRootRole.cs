using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBRmats.Core.Migrations
{
    /// <inheritdoc />
    public partial class AdminRootRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var checkAdminRoleExistsSql = @"
    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE [Name] = 'Admin')
    BEGIN
        INSERT INTO AspNetRoles ([Id], [Name], [NormalizedName])
        VALUES (NEWID(), 'Admin', 'ADMIN')
    END";

            migrationBuilder.Sql(checkAdminRoleExistsSql);

            // SQL to assign "Admin" role to a user with a specific email
            var assignAdminRoleToUserSql = @"
    DECLARE @UserId UNIQUEIDENTIFIER;
    DECLARE @RoleId UNIQUEIDENTIFIER;

    SET @UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'valerii.deineka@oa.edu.ua');
    SET @RoleId = (SELECT Id FROM AspNetRoles WHERE [Name] = 'Admin');

    IF @UserId IS NOT NULL AND @RoleId IS NOT NULL
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
        BEGIN
            INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);
        END
    END";

            migrationBuilder.Sql(assignAdminRoleToUserSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

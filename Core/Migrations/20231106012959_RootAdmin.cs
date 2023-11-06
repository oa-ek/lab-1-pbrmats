using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBRmats.Core.Migrations
{
    /// <inheritdoc />
    public partial class RootAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var checkAdminRoleExistsSql = @"
    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE [Name] = 'RootAdmin')
    BEGIN
        INSERT INTO AspNetRoles ([Id], [Name], [NormalizedName])
        VALUES (NEWID(), 'RootAdmin', 'ROOTADMIN')
    END";

            migrationBuilder.Sql(checkAdminRoleExistsSql);

            // SQL to assign "Admin" role to a user with a specific email
            var assignRootAdminRoleToUserSql = @"
    DECLARE @UserId UNIQUEIDENTIFIER;
    DECLARE @RoleId UNIQUEIDENTIFIER;

    SET @UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'valerii.deineka@oa.edu.ua');
    SET @RoleId = (SELECT Id FROM AspNetRoles WHERE [Name] = 'RootAdmin');

    IF @UserId IS NOT NULL AND @RoleId IS NOT NULL
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
        BEGIN
            INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);
        END
    END";

            migrationBuilder.Sql(assignRootAdminRoleToUserSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

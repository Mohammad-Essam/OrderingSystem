using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Migrations
{
    /// <inheritdoc />
    public partial class UsersAndRolesSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string userRoleGuid = Guid.NewGuid().ToString();
            string adminRoleGuid = Guid.NewGuid().ToString();
            string adminGuid = Guid.NewGuid().ToString();
            string normalUserGuid = Guid.NewGuid().ToString();
            //admin => admin@site.com  000000
            //user => user@site.com 000000
            migrationBuilder.Sql($"INSERT INTO [dbo].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name]) VALUES (N'{adminGuid}', N'admin', N'ADMIN', N'admin@site.com', N'ADMIN@SITE.COM', 0, N'AQAAAAEAACcQAAAAECdvO71xn7zolxUHipTbobw+VjIarhILDCdZUFHly++PB2XryIw/tylueIAhoRgjmA==', N'WNIF4H6SJVQ2ZQTD4PRVBFADXUBKEDRE', N'8fcdc6f1-04b7-4f32-acfa-100a4a723688', NULL, 0, 0, NULL, 1, 0, N'John Doe')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name]) VALUES (N'{normalUserGuid}', N'user', N'USER', N'user@site.com', N'USER@SITE.COM', 0, N'AQAAAAEAACcQAAAAECdvO71xn7zolxUHipTbobw+VjIarhILDCdZUFHly++PB2XryIw/tylueIAhoRgjmA==', N'WNIF4H6SJVQ2ZQTD4PRVBFADXUBKEDRE', N'8fcdc6f1-04b7-4f32-acfa-100a4a723688', NULL, 0, 0, NULL, 1, 0, N'John Doe')");

            migrationBuilder.Sql($"insert into [dbo].[AspNetRoles](id,name, NormalizedName) values('{userRoleGuid}','user','USER')");
            migrationBuilder.Sql($"insert into [dbo].[AspNetRoles](id,name, NormalizedName) values('{adminRoleGuid}','admin','ADMIN')");

            migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('{adminGuid}','{userRoleGuid}')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('{adminGuid}','{adminRoleGuid}')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('{normalUserGuid}','{userRoleGuid}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from AspNetUserRoles");
            migrationBuilder.Sql("delete from AspNetRoles");
            migrationBuilder.Sql("delete from users");

        }
    }
}

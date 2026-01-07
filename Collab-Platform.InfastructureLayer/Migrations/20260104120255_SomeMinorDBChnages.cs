using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collab_Platform.InfastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class SomeMinorDBChnages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectName",
                table: "Project",
                column: "ProjectName");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Key",
                table: "Permission",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectName",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Permission_Key",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers");
        }
    }
}

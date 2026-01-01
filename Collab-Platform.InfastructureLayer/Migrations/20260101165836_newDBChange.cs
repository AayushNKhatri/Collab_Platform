using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collab_Platform.InfastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class newDBChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Project_ProjectModelProjectId",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_ProjectModelProjectId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ProjectModelProjectId",
                table: "Channel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectModelProjectId",
                table: "Channel",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ProjectModelProjectId",
                table: "Channel",
                column: "ProjectModelProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Project_ProjectModelProjectId",
                table: "Channel",
                column: "ProjectModelProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId");
        }
    }
}

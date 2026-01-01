using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collab_Platform.InfastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class DBCHANGE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Project_ProjectId",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_ProjectId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Channel");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Channel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ProjectId",
                table: "Channel",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Project_ProjectId",
                table: "Channel",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

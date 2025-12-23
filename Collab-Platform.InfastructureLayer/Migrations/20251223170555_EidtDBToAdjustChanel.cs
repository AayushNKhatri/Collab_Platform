using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collab_Platform.InfastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class EidtDBToAdjustChanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Channel",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Channel_TaskId",
                table: "Channel",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Task_TaskId",
                table: "Channel",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Task_TaskId",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_TaskId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Channel");
        }
    }
}

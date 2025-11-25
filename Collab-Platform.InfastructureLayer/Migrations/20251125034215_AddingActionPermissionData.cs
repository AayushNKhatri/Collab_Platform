using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collab_Platform.InfastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddingActionPermissionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                 INSERT INTO ""Permission"" (""Key"",""name"",""Description"",""Category"")
                 VALUES
                 ('task_create', 'Create Task', 'Allow User to create task', 'TaskContoller'),
                 ('task_edit', 'Edit Task', 'Allow User to Edit task', 'TaskContoller'),
                 ('task_delete', 'delete Task', 'Allow User to delete task', 'TaskContoller'),
                 ('task_view', 'view Task', 'Allow User to view task', 'TaskContoller');
             ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

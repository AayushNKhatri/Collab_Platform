using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collab_Platform.InfastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddingActionPermissionDataAll : Migration
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
                 ('task_view', 'view Task', 'Allow User to view task', 'TaskContoller'),
                 ('project_edit', 'Edit Project', 'Allow User to Edit Project', 'ProjectController'),
                 ('chanels_create', 'Create Chanels', 'Allow User to Create Chanels', 'ChanelsController'),
                 ('chanels_edit', 'Edit Chanels', 'Allow User to Edit Chanels', 'ChanelsController'),
                 ('chanels_delete', 'Delete Chanels', 'Allow User to Delete Chanels', 'ChanelsController'),
                 ('chanels_view', 'View Chanels', 'Allow User to View Chanels', 'ChanelsController'),
                 ('subtask_create', 'Create Subtask', 'Allow User to Create Subtask', 'SubtaskController'),
                 ('resource_create', 'Create Resource', 'Allow User to Create Resource', 'ResourceController'),
                 ('resource_edit', 'Edit Resource', 'Allow User to Create Resource', 'ResourceController'),
                 ('resource_delete', 'Delete Resource', 'Allow User to Delete Resource', 'ResourceController');
             ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

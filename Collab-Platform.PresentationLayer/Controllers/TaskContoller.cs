using Collab_Platform.ApplicationLayer.DTO.TaskDto;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collab_Platform.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskContoller : ControllerBase
    {
        private readonly ITaskInterface _taskInterface;
        public TaskContoller(ITaskInterface taskInterface) {
            _taskInterface = taskInterface;
        }
        [HttpPost("Create-Task")]
        public async Task<ActionResult<APIResponse>> CreateTask([FromBody] CreateTaskDTO createTask) {
            await _taskInterface.CreateTask(createTask);
            return Ok(new APIResponse {
                Success = true,
                Messege = "Sucessfully created the Task"
            });
        }
        [HttpGet("{TaskId}")]
        public async Task<ActionResult<APIResponse>> GetTaskByTaskId([FromRoute] Guid TaskId) {
            var result = await _taskInterface.GetTaskById(TaskId);
            return Ok(new APIResponse<TaskDetailDto> {
                Success = true,
                Messege = "Sucessfully retireved the Task Data",
                Data = result
            });
        }
        [HttpGet("FormProject/{ProjectId}")]
        public async Task<ActionResult<APIResponse>> GetTaskByProjectId([FromRoute] Guid ProjectId) {
            var result = await _taskInterface.GetTaskByProject(ProjectId);
            return Ok(new APIResponse<List<TaskDetailDto>> {
                Success = true,
                Messege = "Sucessfully retrived the task form project",
                Data = result
            });
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetTaskByUserID() {
            var result = await _taskInterface.GetTaskByUserID();
            return Ok(new APIResponse<List<TaskDetailDto>> {
                Success = true,
                Messege = "Sucessfully retrived the task form user Id",
                Data = result
            });
        }
        [HttpDelete("{TaskId}")]
        public async Task<ActionResult<APIResponse>> DeleteTask([FromRoute]Guid TaskId) {
            await _taskInterface.DeleteTask(TaskId);
            return Ok(new APIResponse { 
                Success = true,
                Messege = "The task has been deleted"
            });
        }
        [HttpPut("{TaskId}")]
        public async Task<ActionResult<APIResponse>> UpdateTask([FromRoute] Guid TaskId, [FromBody] UpdateTaskDto updateTask) {
            await _taskInterface.UpdateTask(TaskId, updateTask);
            return Ok(new APIResponse
            {
                Success = true,
                Messege = "Sucessfully updated the Task"
            });
        }
     }
}

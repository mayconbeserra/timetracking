using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Visma.TimeTracking.AppService;
using Visma.TimeTracking.API.Models;

namespace Visma.TimeTracking.API.Controllers
{
    [Route("api/v1/activities")]
    public class ActivitiesController : Controller
    {
        public IActivityService ActivityService { get; }

        public ActivitiesController(IActivityService activityService)
        {
            ActivityService = activityService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActivityInput input)
        {
            if (input == null) return BadRequest();
            if (string.IsNullOrEmpty(input.ProjectId)) return BadRequest();

            await ActivityService.StartActivity(input.ProjectId, DateTime.UtcNow, HttpContext.TraceIdentifier, input.Description);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ActivityInput input)
        {
            if (!input.StartDate.HasValue || !input.EndDate.HasValue) return BadRequest();

            var result = await ActivityService.AdjustActivity(
                id,
                input.StartDate.Value,
                input.EndDate.Value,
                HttpContext.TraceIdentifier,
                input.Description);

            return result > 0 ? (IActionResult) Ok() : BadRequest();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] ActivityInput input)
        {
            var result = await ActivityService.PauseActivity(
                id,
                DateTime.UtcNow,
                HttpContext.TraceIdentifier,
                input.Description);

            return result > 0 ? (IActionResult) Ok() : BadRequest();
        }
    }
}
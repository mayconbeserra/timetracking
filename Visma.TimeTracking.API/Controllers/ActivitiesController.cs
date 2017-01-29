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

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

            await ActivityService.AdjustActivity(id, input.StartDate.Value, input.EndDate.Value, HttpContext.TraceIdentifier, input.Description);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] ActivityInput input)
        {
            await ActivityService.PauseActivity(id, DateTime.UtcNow, HttpContext.TraceIdentifier, input.Description);

            return Ok();
        }
    }
}
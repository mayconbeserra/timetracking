using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.Projections.Contexts;
using Visma.TimeTracking.Projections.Entities;

namespace Visma.TimeTracking.API.Controllers
{
    [Route("api/v1/projects")]
    public class ProjectsController : Controller
    {
        protected TimerTrackingDbContext Context;

        public ProjectsController(TimerTrackingDbContext context)
        {
            Context = context;
        }

        [HttpGet]
        public async Task<IList<Project>> Get()
        {
            return await Context.Projects.Include(x => x.Activities).ToListAsync();
        }
    }
}
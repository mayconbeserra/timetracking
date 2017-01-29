using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.Projections.Contexts;
using Visma.TimeTracking.Projections.Entities;

namespace Visma.TimeTracking.API.Controllers
{
    [Route("api/v1/customers")]
    public class CustomersController : Controller
    {
        public TimerTrackingDbContext Context { get; }

        public CustomersController(TimerTrackingDbContext context)
        {
            Context = context;
        }

        [HttpGet()]
        public async Task<IList<Customer>> Get()
        {
            return await Context.Customers.ToListAsync();
        }
    }
}
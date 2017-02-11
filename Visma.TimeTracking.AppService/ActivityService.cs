using System;
using System.Threading.Tasks;
using Visma.TimeTracking.Domain.Activity;

namespace Visma.TimeTracking.AppService
{
    public class ActivityService : ServiceBase, IActivityService
    {
        public ActivityService(IDomainRepository repository) : base(repository)
        {
        }

        public async Task<int> StartActivity(string projectId, DateTime startDate, string requestId, string description)
        {
            return await Repository.Save(new ActivityModel().Start(projectId, startDate, description, "Creator", requestId));
        }

        public async Task<int> PauseActivity(string aggregateId, DateTime endDate, string requestId, string description)
        {
            var activity = await Repository.Load<ActivityModel>(aggregateId);

            if (activity == null) return -1;

            return await Repository.Save(activity.Pause(endDate, description, "Creator", requestId));
        }

        public async Task<int> AdjustActivity(string aggregate, DateTime startDate, DateTime endDate, string requestId, string description)
        {
            var activity = await Repository.Load<ActivityModel>(aggregate);

            if (activity == null) return -1;

            return await Repository.Save(activity.Adjust(startDate, endDate, description, "Creator", requestId));
        }
    }
}
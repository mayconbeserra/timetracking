using System;
using System.Threading.Tasks;

namespace Visma.TimeTracking.AppService
{
    public interface IActivityService
    {
        Task<int> StartActivity(string projectId, DateTime startDate, string requestId, string description);
        Task<int> PauseActivity(string aggregateId, DateTime endDate, string requestId, string description);
        Task<int> AdjustActivity(string aggregate, DateTime startDate, DateTime endDate, string requestId, string description);
    }
}
using System;
using NSubstitute;
using Visma.TimeTracking.AppService;
using Visma.TimeTracking.Domain.Activity;
using Xunit;

namespace Visma.TimeTracking.Tests.AppService
{
    public class WhenActivityService
    {
        [Theory]
        [InlineData("Project1", "Request1", "Desc1")]
        [InlineData("Project2", "Request2", "Desc2")]
        public void StartActivity_Then_An_Event_Should_Be_Sent(string projectId, string requestId, string description)
        {
            var mockRepository = Substitute.For<IDomainRepository>();
            var activityService = new ActivityService(mockRepository);

            activityService.StartActivity(projectId, DateTime.UtcNow, requestId, description);

            mockRepository.Received(1).Save(Arg.Any<ActivityModel>());
        }

        [Theory]
        [InlineData("Aggregate1", "Request1", "Desc1")]
        [InlineData("Aggregate1", "Request2", "Desc2")]
        public void PauseActivity_Then_An_Event_Should_Be_Sent(string aggregate, string requestId, string description)
        {
            var mockRepository = Substitute.For<IDomainRepository>();
            var activityService = new ActivityService(mockRepository);

            mockRepository.Load<ActivityModel>(aggregate).Returns(
                new ActivityModel()
                    .Start("Proj1", DateTime.UtcNow, description, "Creator", requestId));

            activityService.PauseActivity(aggregate, DateTime.UtcNow, requestId, description);

            mockRepository.Received(1).Load<ActivityModel>(aggregate);
            mockRepository.Received(1).Save(Arg.Any<ActivityModel>());
        }

        [Theory]
        [InlineData("Aggregate1", "Request1", "Desc1")]
        [InlineData("Aggregate1", "Request2", "Desc2")]
        public void AdjustActivity_Then_An_Event_Should_Be_Sent(string aggregate, string requestId, string description)
        {
            var mockRepository = Substitute.For<IDomainRepository>();
            var activityService = new ActivityService(mockRepository);

            mockRepository.Load<ActivityModel>(aggregate).Returns(
                new ActivityModel()
                    .Start("Proj1", DateTime.UtcNow, description, "Creator", requestId));

            activityService.AdjustActivity(aggregate, DateTime.UtcNow, DateTime.UtcNow, requestId, description);

            mockRepository.Received(1).Load<ActivityModel>(aggregate);
            mockRepository.Received(1).Save(Arg.Any<ActivityModel>());
        }
    }
}
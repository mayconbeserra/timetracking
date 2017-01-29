using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Visma.TimeTracking.Domain.Activity;
using Xunit;

namespace Visma.TimeTracking.Tests.Repositories
{
    public class WhenActivityRepository : RepositoryBase
    {
        [Fact]
        public void Is_Instantiate_Then_It_Should_Load_Correctly()
        {
            // Arrange
            new ActivityModel().Start("Project1", DateTime.UtcNow, "Desc", string.Empty, string.Empty);
            var repository = Container.GetInstance<IDomainRepository>();

            // Act & Assert
            repository.Should().NotBeNull();
        }

        [Fact]
        public async void Is_Instantiate_And_A_New_Activity_Is_Created_Then_An_Entity_Should_Saved()
        {
            // Arrange
            var activity = new ActivityModel().Start(CreateProject(), DateTime.UtcNow, "Desc", string.Empty, string.Empty);
            var repository = Container.GetInstance<IDomainRepository>();

            // Act & Assert
            await repository.Save(activity);
        }

        [Fact]
        public async void Load_An_Entity_Then_It_Should_Return_It()
        {
            // Arrange
            var activity = new ActivityModel().Start(CreateProject(), DateTime.UtcNow, "Desc", string.Empty, string.Empty);
            var repository = Container.GetInstance<IDomainRepository>();

            // Act
            await repository.Save(activity);

            // Assert
            var entityLoaded = await repository.Load<ActivityModel>(activity.Id);

            entityLoaded.Id.Should().Be(activity.Id);
            entityLoaded.Version.Should().Be(activity.Version);
        }

        [Theory]
        [InlineData("Description", "Creator", "Correlation", 60)]
        public async void Load_An_Entity_And_Start_And_Pause_TheActivity(string description, string creator, string correlation, double expectedMinutes)
        {
            // Arrange
            var startDate = new DateTime(2018, 01, 01, 10, 0, 0);
            var endDate = new DateTime(2018, 01, 01, (int)(expectedMinutes / 60), 0, 0);
            var projectId = CreateProject();
            var activity = new ActivityModel().Start(projectId, startDate, "Desc", creator, correlation);
            var repository = Container.GetInstance<IDomainRepository>();

            // Act
            activity.Pause(endDate, description, creator, correlation);
            await repository.Save(activity);

            var entityLoaded = await repository.Load<ActivityModel>(activity.Id);
            var projection = await TimerTrackingDbContext.Activities.FirstAsync(x => x.Id == activity.Id);

            // Assert Model
            entityLoaded.Id.Should().Be(activity.Id);
            entityLoaded.Version.Should().Be(activity.Version);

            // Assert Projection
            projection.StartDate.Should().Be(startDate);
            projection.EndDate.Should().Be(endDate);
            projection.ProjectId.Should().Be(projectId);
            projection.CreatorId.Should().Be(creator);
            projection.Description.Should().Be(description);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Empty")]
        public async void TryToLoad_An_Entity_And_It_DoesNot_Exist_Then_It_Should_Return_Null(string id)
        {
            var repository = Container.GetInstance<IDomainRepository>();

            Assert.Null(await repository.Load<ActivityModel>(id));
        }
    }
}
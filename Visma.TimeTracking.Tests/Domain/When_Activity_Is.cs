using System;
using System.Linq;
using FluentAssertions;
using Visma.TimeTracking.Domain.Activity;
using Visma.TimeTracking.Domain.Events.V1;
using Visma.TimeTracking.EventSourcing.Events;
using Xunit;

namespace Visma.TimeTracking.Tests.Domain
{
    public class WhenActivityIs : AggregateTest<ActivityModel, ActivityState>
    {
        private ActivityModel Activity => Aggregate;

        [Theory]
        [InlineData("Activity Started 1", "Creator1", "CorrelationId")]
        [InlineData("Activity Started 2", "Creator1", "CorrelationId")]
        public void Started_Then_StartDate_Should_Be_Set(string description, string creator, string correlationId)
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var projectId = Guid.NewGuid().ToString();

            // Act
            Activity.Start(projectId, startDate, description, creator, correlationId);

            // Assert
            State.Id.Should().NotBeNullOrEmpty();
            State.ProjectId.Should().Be(projectId);
            State.StartDate.Should().Be(startDate);
            State.Description.Should().Be(description);

            var @event = UncommittedChanges.First() as ActivityStarted;

            @event?.CreatorId.Should().Be(creator);
            @event?.CorrelationId.Should().Be(correlationId);
        }

        [Theory]
        [InlineData("", true, "description")]
        [InlineData("project1", false, "description")]
        [InlineData("project1", true, "")]
        public void Started_And_One_Of_Params_Is_Null_Or_Empty_Then_Throws_An_Exception(
            string projectId,
            bool withStartDate,
            string description)
        {
            // Arrange
            var startDate = withStartDate ? DateTime.UtcNow : DateTime.MaxValue;

            // Act
            Assert.Throws<ArgumentNullException>(() => Activity.Start(projectId, startDate, description, string.Empty, string.Empty));
        }

        [Theory]
        [InlineData("Activity Paused 1", "Creator1", "CorrelationId", 60)]
        [InlineData("Activity Paused 2", "Creator2", "CorrelationId", 120)]
        [InlineData("Activity Paused 2", "Creator3", "CorrelationId", 180)]
        public void Paused_Then_EndDate_Should_Be_Set(string description, string creator, string correlationId, double minutes)
        {
            // Arrange
            var aggregateId = Guid.NewGuid().ToString();
            var projectId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2017, 02, 01, 0, 0,0);
            var created = new ActivityStarted(aggregateId, projectId, startDate, description, creator, correlationId);

            LoadFromHistory(aggregateId, new IDomainEvent[] { created });

            var endDate = new DateTime(2017, 02, 01, (int) (minutes / 60), 0,0);

            // Act
            Activity.Pause(endDate, description, creator, correlationId);

            // Assert
            State.Id.Should().Be(aggregateId);
            State.ProjectId.Should().Be(projectId);
            State.StartDate.Should().Be(startDate);
            State.EndDate.Should().Be(endDate);
            State.TotalInMinutes.Should().Be(minutes);
            State.Description.Should().Be(description);

            var @event = UncommittedChanges.First() as ActivityPaused;
            @event?.ProjectId.Should().Be(projectId);
            @event?.AggregateId.Should().Be(aggregateId);
            @event?.CreatorId.Should().Be(creator);
            @event?.CorrelationId.Should().Be(correlationId);
            @event?.TotalInMinutes.Should().Be(minutes);
        }

        [Theory]
        [InlineData(false, "description")]
        [InlineData(true, "")]
        public void Paused_And_One_Of_Params_Is_Null_Or_Empty_Then_Throws_An_Exception(
            bool withEndDate,
            string description)
        {
            // Arrange
            var aggregateId = Guid.NewGuid().ToString();
            var projectId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2017, 02, 01, 0, 0,0);
            var created = new ActivityStarted(aggregateId, projectId, startDate, description, string.Empty, string.Empty);

            LoadFromHistory(aggregateId, new IDomainEvent[] { created });
            var endDate = withEndDate ? DateTime.UtcNow : DateTime.MaxValue;

            // Act
            Assert.Throws<ArgumentNullException>(() => Activity.Pause(endDate, description, string.Empty, string.Empty));
        }

        [Theory]
        [InlineData("Activity Paused 1", "Creator1", "CorrelationId", 60)]
        [InlineData("Activity Paused 2", "Creator2", "CorrelationId", 120)]
        [InlineData("Activity Paused 2", "Creator3", "CorrelationId", 180)]
        public void Paused_And_Aggregate_Was_Already_Paused_Then_Throws_An_Exception(string description,
            string creator,
            string correlationId,
            double minutes)
        {
            // Arrange
            var aggregateId = Guid.NewGuid().ToString();
            var projectId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2017, 02, 01, 0, 0,0);
            var created = new ActivityStarted(aggregateId, projectId, startDate, description, creator, correlationId);

            LoadFromHistory(aggregateId, new IDomainEvent[] { created });

            var endDate = new DateTime(2017, 02, 01, (int) (minutes / 60), 0,0);

            // Act
            Activity.Pause(endDate, description, creator, correlationId);

            // Arrange
            var newEndDate = DateTime.UtcNow;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => Activity.Pause(newEndDate, "Description", string.Empty, string.Empty));
        }

        [Fact]
        public void Paused_And_AggregateId_Is_Null_Or_Empty_Then_Throws_An_Exception()
        {
            // Arrange
            var endDate = DateTime.UtcNow;

            // Act
            Assert.Throws<InvalidOperationException>(() => Activity.Pause(endDate, "Description", string.Empty, string.Empty));
        }

        [Theory]
        [InlineData("Activity Adjusted 1", "Creator1", "CorrelationId", 60)]
        [InlineData("Activity Adjusted 2", "Creator2", "CorrelationId", 120)]
        [InlineData("Activity Adjusted 2", "Creator3", "CorrelationId", 180)]
        [InlineData("Activity Adjusted 2", "Creator3", "CorrelationId", 360)]
        public void Adjusted_Then_StartDate_And_EndDate_Should_Be_Set(string description,
            string creator,
            string correlationId,
            double minutes)
        {
            // Arrange
            var aggregateId = Guid.NewGuid().ToString();
            var projectId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2017, 02, 01, 0, 0,0);
            var created = new ActivityStarted(aggregateId, projectId, startDate, description, "Creator", "Correlation");

            LoadFromHistory(aggregateId, new IDomainEvent[] { created });

            var newStartDate = new DateTime(2018, 02, 01, 0, 0,0);
            var endDate = new DateTime(2018, 02, 01, (int) (minutes / 60), 0,0);

            // Act
            Activity.Adjust(newStartDate, endDate, description, creator, correlationId);

            // Assert
            State.Id.Should().Be(aggregateId);
            State.ProjectId.Should().Be(projectId);
            State.StartDate.Should().Be(newStartDate);
            State.EndDate.Should().Be(endDate);
            State.TotalInMinutes.Should().Be(minutes);
            State.Description.Should().Be(description);

            var @event = UncommittedChanges.First() as ActivityAdjusted;
            @event?.CreatorId.Should().Be(creator);
            @event?.ProjectId.Should().Be(projectId);
            @event?.CorrelationId.Should().Be(correlationId);
            @event?.TotalInMinutes.Should().Be(minutes);
        }

        [Theory]
        [InlineData(false, true, "description")]
        [InlineData(true, false, "description")]
        [InlineData(true, true, "")]
        public void Adjusted_And_One_Of_Params_Is_Null_Or_Empty_Then_Throws_An_Exception(
            bool withStartDate,
            bool withEndDate,
            string description)
        {
            // Arrange
            var aggregateId = Guid.NewGuid().ToString();
            var projectId = Guid.NewGuid().ToString();
            var initialDate = new DateTime(2017, 02, 01, 0, 0,0);
            var created = new ActivityStarted(aggregateId, projectId, initialDate, description, string.Empty, string.Empty);

            LoadFromHistory(aggregateId, new IDomainEvent[] { created });

            var startDate = withStartDate ? DateTime.UtcNow : DateTime.MaxValue;
            var endDate = withEndDate ? DateTime.UtcNow : DateTime.MaxValue;

            // Act
            Assert.Throws<ArgumentNullException>(() => Activity.Adjust(startDate, endDate, description, string.Empty, string.Empty));
        }

        [Fact]
        public void Adjusted_And_AggregateId_Is_Null_Or_Empty_Then_Throws_An_Exception()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow;

            // Act
            Assert.Throws<InvalidOperationException>(() => Activity.Adjust(startDate, endDate, "Description", string.Empty, string.Empty));
        }
    }
}
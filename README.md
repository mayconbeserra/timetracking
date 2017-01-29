# Welcome to TimeTracking (Web API)

This cool API is going to be used for tracking time.

For now, I just created some end points for starting, pausing and adjusting time for one project, however, it should be easily add more features.

# Stack and concepts used

+ Clean architecture (Mix of onion architecture);
+ CQRS
+ Event Sourcing
+ DDD
+ .NET Core 1.1
+ XUnit
+ EF Core
+ Docker
+ Migrations
+ Postgres

# How to run?

1. git clone git@github.com:mayconbeserra/timetracking.git
2. cd timetracking
3. dotnet restore
4. docker run --name postgres-timetracking -p 5432:5432 -e POSTGRES_PASSWORD=sql -e POSTGRES_DB=timertracking -d postgres:latest
5. dotnet run -p Visma.TimeTracking.MigrationsEventSourcing
6. dotnet run -p Visma.TimeTracking.MigrationsProjection

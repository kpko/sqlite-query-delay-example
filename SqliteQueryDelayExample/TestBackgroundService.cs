using Akka.Actor;
using Akka.Persistence.Query;
using Akka.Persistence.Sql.Query;

namespace SqliteQueryDelayExample;

public class TestBackgroundService : BackgroundService
{
    private readonly ActorSystem _actorSystem;
    private readonly ILogger<TestBackgroundService> _logger;

    public TestBackgroundService(ActorSystem actorSystem, ILogger<TestBackgroundService> logger)
    {
        _actorSystem = actorSystem;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting query...");

        var query = PersistenceQuery
            .Get(_actorSystem)
            .ReadJournalFor<SqlReadJournal>(SqlReadJournal.Identifier)
            .CurrentEventsByTag("tag", Offset.NoOffset());

        await foreach (var ee in query.RunAsAsyncEnumerable(_actorSystem).WithCancellation(stoppingToken))
        {
            _logger.LogInformation("Got event {Event}", ee.Event);
        }
    }
}
using Akka.Persistence;

namespace SqliteQueryDelayExample;

public record Initialize;
public record Initialized;
public class TestActor : ReceivePersistentActor
{
    public override string PersistenceId { get; } = "test";

    private bool _isInitialized;

    public TestActor()
    {
        Command<Initialize>(_ =>
        {
            if (!_isInitialized)
            {
                Persist(new Initialized(), Apply);
            }

            Sender.Tell("ok", Self);
        });

        RecoverAny(Apply);
    }

    private void Apply(object e)
    {
        if (e is Initialized)
        {
            _isInitialized = true;
        }
    }
}
using Akka.Persistence.Journal;

namespace SqliteQueryDelayExample;

public class TaggingWriteEventAdapter : IWriteEventAdapter
{
    public string Manifest(object evt) => string.Empty;

    public object ToJournal(object evt) => evt switch
    {
        Initialized e => new Tagged(e, new[] { "tag" }),
        _             => evt
    };
}
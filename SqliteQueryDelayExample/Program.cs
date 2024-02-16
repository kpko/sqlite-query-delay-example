using Akka.Actor;
using Akka.Hosting;
using Akka.Persistence.Sql.Hosting;
using LinqToDB;
using SqliteQueryDelayExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAkka("test", (akka, _) =>
{
    akka.WithSqlPersistence("DataSource=test.db",
        providerName: ProviderName.SQLite,
        journalBuilder: jb => jb.AddWriteEventAdapter<TaggingWriteEventAdapter>("tagging", new[] { typeof(object) })
    );
});
builder.Services.AddHostedService<TestBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use TestActor to simulate a command
var system = app.Services.GetRequiredService<ActorSystem>();
var actor = system.ActorOf(Props.Create<TestActor>());
await actor.Ask(new Initialize());

app.Run();
using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Data.Common;
using Testcontainers.MsSql;

namespace JornadaMilhas.Test.Integracao;

public class DatabaseFixture : IAsyncLifetime
{
    public JornadaMilhasContext Context { get; private set; }
    private Respawner _respawner;
    private DbConnection _connection;
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server")
        .Build();

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString())
            .Options;

        Context = new JornadaMilhasContext(options);

        await Context.Database.MigrateAsync();

        _connection = Context.Database.GetDbConnection();
        await _connection.OpenAsync();

        var respawnerOptions = new RespawnerOptions
        {
            SchemasToInclude = new[]
            {
                "dbo"
            }
        };

        _respawner = await Respawner.CreateAsync(_connection, respawnerOptions);
    }

    public Task ResetDatabase()
    {
        return _respawner.ResetAsync(_connection);
    }

    public async Task DisposeAsync()
    {
        await Context.DisposeAsync();

        await _connection.CloseAsync();
        await _msSqlContainer.StopAsync();
    }

    public void CriaDadosFake()
    {
        Periodo periodo = new PeriodoDataBuilder().Build();

        var rota = new Rota("Curitiba", "São Paulo");

        var fakerOferta = new Faker<OfertaViagem>()
            .CustomInstantiator(f => new OfertaViagem(
                rota,
                new PeriodoDataBuilder().Build(),
                100 * f.Random.Int(1, 100))
            )
            .RuleFor(o => o.Desconto, f => 40)
            .RuleFor(o => o.Ativa, f => true);

        var lista = fakerOferta.Generate(200);
        Context.OfertasViagem.AddRange(lista);
        Context.SaveChanges();
    }
}

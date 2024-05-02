using JornadaMilhas.Dados;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;

[Collection(nameof(DatabaseCollection))]
public class OfertaViagemDalRecuperarPorId : IAsyncLifetime
{
    private readonly DatabaseFixture fixture;
    private readonly JornadaMilhasContext _context;
    private readonly Func<Task> _resetDatabase;

    public OfertaViagemDalRecuperarPorId(
        DatabaseFixture fixture,
        ITestOutputHelper output
    )
    {
        this.fixture = fixture;
        _context = fixture.Context;
        _resetDatabase = fixture.ResetDatabase;
        output.WriteLine(_context.GetHashCode().ToString());
    }
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();

    [Fact]
    public void RetornaNuloQuandoIdInexistente()
    {
        //arrange
        var dal = new OfertaViagemDAL(_context);

        //act
        var ofertaRecuperada = dal.RecuperarPorId(-2);

        //assert
        Assert.Null(ofertaRecuperada);
    }

}

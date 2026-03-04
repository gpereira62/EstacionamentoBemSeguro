using EstacionamentoBemSeguro.Models;
using static EstacionamentoBemSeguro.Models.Vaga;

namespace EstacionamentoBemSeguro.Tests;

public class EstacionamentoTests
{
    private static Estacionamento CriarEstacionamento(int pequenas = 2, int medias = 3, int grandes = 1, double precoHora = 10.0)
    {
        var e = new Estacionamento
        {
            QtdeVagaPequena = pequenas,
            QtdeVagaMedia = medias,
            QtdeVagaGrande = grandes,
            PrecoHora = precoHora
        };
        e.CriarVagas();
        return e;
    }

    private static Veiculo CriarVeiculo(Veiculo.Type tipo, int minutosAtras = 0)
    {
        var v = new Veiculo(tipo);
        if (minutosAtras > 0)
            v.DataHoraEntrada = DateTime.Now.AddMinutes(-minutosAtras);
        return v;
    }

    // ===================== PodeEstacionar =====================

    [Fact]
    public void PodeEstacionarMoto_ComVagasDisponiveis_RetornaTrue()
    {
        var e = CriarEstacionamento(pequenas: 1, medias: 0, grandes: 0);
        Assert.True(e.PodeEstacionarMoto());
    }

    [Fact]
    public void PodeEstacionarMoto_SemVagas_RetornaFalseEAdicionaAviso()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 0, grandes: 0);
        bool resultado = e.PodeEstacionarMoto();
        Assert.False(resultado);
        Assert.Single(e.Avisos);
    }

    [Fact]
    public void PodeEstacionarCarro_ComVagaMedia_RetornaTrue()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 1, grandes: 0);
        Assert.True(e.PodeEstacionarCarro());
    }

    [Fact]
    public void PodeEstacionarCarro_ComSomenteVagaGrande_RetornaTrue()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 0, grandes: 1);
        Assert.True(e.PodeEstacionarCarro());
    }

    [Fact]
    public void PodeEstacionarCarro_SemVagas_RetornaFalseEAdicionaAviso()
    {
        var e = CriarEstacionamento(pequenas: 2, medias: 0, grandes: 0);
        bool resultado = e.PodeEstacionarCarro();
        Assert.False(resultado);
        Assert.Single(e.Avisos);
    }

    [Fact]
    public void PodeEstacionarVan_ComVagaGrande_RetornaTrue()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 0, grandes: 1);
        Assert.True(e.PodeEstacionarVan());
    }

    [Fact]
    public void PodeEstacionarVan_ComTresVagasMedias_RetornaTrue()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 3, grandes: 0);
        Assert.True(e.PodeEstacionarVan());
    }

    [Fact]
    public void PodeEstacionarVan_ComDuasVagasMedias_RetornaFalse()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 2, grandes: 0);
        bool resultado = e.PodeEstacionarVan();
        Assert.False(resultado);
        Assert.Single(e.Avisos);
    }

    [Fact]
    public void PodeEstacionarVan_SemVagas_RetornaFalseEAdicionaAviso()
    {
        var e = CriarEstacionamento(pequenas: 2, medias: 0, grandes: 0);
        bool resultado = e.PodeEstacionarVan();
        Assert.False(resultado);
        Assert.Single(e.Avisos);
    }

    // ===================== EstacionarVeiculo =====================

    [Fact]
    public void EstacionarMoto_OcupaPequenoEAdicionaAviso()
    {
        var e = CriarEstacionamento(pequenas: 1, medias: 1, grandes: 1);
        var moto = CriarVeiculo(Veiculo.Type.Moto);

        e.EstacionarVeiculo(moto);

        var vagaOcupada = e.Vagas.Single(v => v.Status == State.Ocupada);
        Assert.Equal(Vaga.Type.Pequena, vagaOcupada.Tipo);
        Assert.Contains(e.Avisos, a => a.Mensagem.Contains("sucesso"));
    }

    [Fact]
    public void EstacionarMoto_SemPequena_OcupaMedia()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 1, grandes: 0);
        var moto = CriarVeiculo(Veiculo.Type.Moto);

        e.EstacionarVeiculo(moto);

        var vagaOcupada = e.Vagas.Single(v => v.Status == State.Ocupada);
        Assert.Equal(Vaga.Type.Media, vagaOcupada.Tipo);
    }

    [Fact]
    public void EstacionarCarro_OcupaMedia()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 1, grandes: 1);
        var carro = CriarVeiculo(Veiculo.Type.Carro);

        e.EstacionarVeiculo(carro);

        var vagaOcupada = e.Vagas.Single(v => v.Status == State.Ocupada);
        Assert.Equal(Vaga.Type.Media, vagaOcupada.Tipo);
    }

    [Fact]
    public void EstacionarCarro_SemMedia_OcupaGrande()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 0, grandes: 1);
        var carro = CriarVeiculo(Veiculo.Type.Carro);

        e.EstacionarVeiculo(carro);

        var vagaOcupada = e.Vagas.Single(v => v.Status == State.Ocupada);
        Assert.Equal(Vaga.Type.Grande, vagaOcupada.Tipo);
    }

    [Fact]
    public void EstacionarVan_OcupaGrande()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 3, grandes: 1);
        var van = CriarVeiculo(Veiculo.Type.Van);

        e.EstacionarVeiculo(van);

        var vagaOcupada = e.Vagas.Single(v => v.Status == State.Ocupada);
        Assert.Equal(Vaga.Type.Grande, vagaOcupada.Tipo);
        Assert.False(van.OcupaVagaMedia);
    }

    [Fact]
    public void EstacionarVan_SemGrande_OcupaTresMedias()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 3, grandes: 0);
        var van = CriarVeiculo(Veiculo.Type.Van);

        e.EstacionarVeiculo(van);

        var vagasOcupadas = e.Vagas.Where(v => v.Status == State.Ocupada).ToList();
        Assert.Equal(3, vagasOcupadas.Count);
        Assert.All(vagasOcupadas, v => Assert.Equal(Vaga.Type.Media, v.Tipo));
        Assert.True(van.OcupaVagaMedia);
        Assert.Contains(e.Avisos, a => a.Mensagem.Contains("3 vagas médias"));
    }

    // ===================== ExcluirVeiculo =====================

    [Fact]
    public void ExcluirVeiculo_LiberaVaga()
    {
        var e = CriarEstacionamento(pequenas: 1, medias: 0, grandes: 0);
        var moto = CriarVeiculo(Veiculo.Type.Moto);
        e.EstacionarVeiculo(moto);
        e.Avisos.Clear();

        e.ExcluirVeiculo(moto);

        Assert.Equal(0, e.TotalVagasOcupadas());
    }

    [Fact]
    public void ExcluirVan_OcupandoTresMedias_LiberaTodasAsVagas()
    {
        var e = CriarEstacionamento(pequenas: 0, medias: 3, grandes: 0);
        var van = CriarVeiculo(Veiculo.Type.Van);
        e.EstacionarVeiculo(van);
        e.Avisos.Clear();

        e.ExcluirVeiculo(van);

        Assert.Equal(0, e.TotalVagasOcupadas());
        Assert.Equal(3, e.TotalVagasDisponiveis());
    }

    // ===================== InfoPagamentoSaida =====================

    [Fact]
    public void InfoPagamentoSaida_MenosDeUmaHora_CobraUmaHora()
    {
        var e = CriarEstacionamento(precoHora: 10.0);
        var veiculo = CriarVeiculo(Veiculo.Type.Carro, minutosAtras: 30);

        var (valor, _) = e.InfoPagamentoSaida(veiculo);

        Assert.Equal(10.0, valor);
    }

    [Fact]
    public void InfoPagamentoSaida_DuasHoras_CobraDuasHoras()
    {
        var e = CriarEstacionamento(precoHora: 10.0);
        var veiculo = CriarVeiculo(Veiculo.Type.Carro, minutosAtras: 120);

        var (valor, _) = e.InfoPagamentoSaida(veiculo);

        Assert.Equal(20.0, valor);
    }

    [Fact]
    public void InfoPagamentoSaida_RetornaTempoCorreto()
    {
        var e = CriarEstacionamento(precoHora: 5.0);
        var veiculo = CriarVeiculo(Veiculo.Type.Moto, minutosAtras: 90);

        var (_, tempo) = e.InfoPagamentoSaida(veiculo);

        Assert.True(tempo.TotalMinutes >= 89 && tempo.TotalMinutes <= 91);
    }
}

using EstacionamentoBemSeguro.Models;

namespace EstacionamentoBemSeguro.Tests;

public class AvisoTests
{
    private static Estacionamento CriarEstacionamento(int pequenas, int medias, int grandes)
    {
        var e = new Estacionamento
        {
            QtdeVagaPequena = pequenas,
            QtdeVagaMedia = medias,
            QtdeVagaGrande = grandes
        };
        e.CriarVagas();
        return e;
    }

    [Fact]
    public void ChecarAvisos_EstacionamentoVazio_AdicionaAvisoVazio()
    {
        var e = CriarEstacionamento(1, 1, 1);
        Aviso.ChecarAvisos(e);
        Assert.Contains(e.Avisos, a => a.Mensagem.Contains("vazio"));
    }

    [Fact]
    public void ChecarAvisos_EstacionamentoCheio_AdicionaAvisoCheio()
    {
        var e = CriarEstacionamento(1, 0, 0);
        e.EstacionarVeiculo(new Veiculo(Veiculo.Type.Moto));
        e.Avisos.Clear();

        Aviso.ChecarAvisos(e);

        Assert.Contains(e.Avisos, a => a.Mensagem.Contains("cheio"));
    }

    [Fact]
    public void ChecarAvisos_VagasPequenasOcupadas_AdicionaAviso()
    {
        var e = CriarEstacionamento(pequenas: 1, medias: 1, grandes: 0);
        e.EstacionarVeiculo(new Veiculo(Veiculo.Type.Moto));
        e.Avisos.Clear();

        Aviso.ChecarAvisos(e);

        Assert.Contains(e.Avisos, a => a.Mensagem.Contains("pequenas"));
    }

    [Fact]
    public void ChecarAvisos_VagasMediasOcupadas_AdicionaAviso()
    {
        var e = CriarEstacionamento(pequenas: 1, medias: 1, grandes: 0);
        e.EstacionarVeiculo(new Veiculo(Veiculo.Type.Carro));
        e.Avisos.Clear();

        Aviso.ChecarAvisos(e);

        Assert.Contains(e.Avisos, a => a.Mensagem.Contains("médias"));
    }

    [Fact]
    public void ChecarAvisos_VagasGrandesOcupadas_AdicionaAviso()
    {
        var e = CriarEstacionamento(pequenas: 1, medias: 0, grandes: 1);
        e.EstacionarVeiculo(new Veiculo(Veiculo.Type.Van));
        e.Avisos.Clear();

        Aviso.ChecarAvisos(e);

        Assert.Contains(e.Avisos, a => a.Mensagem.Contains("grandes"));
    }
}

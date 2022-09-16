using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EstacionamentoBemSeguro.Models.Vaga;

namespace EstacionamentoBemSeguro.Models
{
    internal class Estacionamento
    {
        public List<Vaga> Vagas { get; set; } = new();
        public List<Veiculo> Veiculos { get; set; } = new();
        public double PrecoHora { get; set; }
        public int QtdeVagaPequena { get; set; }
        public int QtdeVagaMedia { get; set; }
        public int QtdeVagaGrande { get; set; }
        public double Caixa { get; set; }

        public void CriarVagas()
        {
            Vaga vagaPequena = new Vaga(Vaga.Type.Pequena);
            Vaga vagaMedia = new Vaga(Vaga.Type.Media);
            Vaga vagaGrande = new Vaga(Vaga.Type.Grande);

            for (int i = 0; i < QtdeVagaPequena; i++) { Vagas.Add(vagaPequena); }
            for (int i = 0; i < QtdeVagaMedia; i++) { Vagas.Add(vagaMedia); }
            for (int i = 0; i < QtdeVagaGrande; i++) { Vagas.Add(vagaGrande); }
        }

        public int TotalVagas()
        {
            return Vagas.Count();
        }

        public int TotalVagasDisponiveis()
        {
            return Vagas.Where(x => x.Status == State.Disponivel).Count();
        }

        public int TotalVagasOcupadas()
        {
            return Vagas.Where(x => x.Status == State.Ocupada).Count();
        }

        public Vaga? EncontrarVagaPequena()
        {
            return Vagas.Where(x => x.Tipo == Vaga.Type.Pequena).Where(y => y.Status == State.Disponivel).FirstOrDefault();
        }

        public Vaga? EncontrarVagaMedia()
        {
            return Vagas.Where(x => x.Tipo == Vaga.Type.Media).Where(y => y.Status == State.Disponivel).FirstOrDefault();
        }

        public Vaga? EncontrarVagaGrande()
        {
            return Vagas.Where(x => x.Tipo == Vaga.Type.Grande).Where(y => y.Status == State.Disponivel).FirstOrDefault();
        }

        public void EstacionarVeiculo(Veiculo veiculo)
        {
            if (veiculo.Tipo == Veiculo.Type.Moto)
            {
                Vaga? vaga = EncontrarVagaPequena();
                this.Vagas.Single(x => x.Id == vaga.Id).Veiculo = veiculo;
            }
            else if (veiculo.Tipo == Veiculo.Type.Carro)
            {
                Vaga? vaga = EncontrarVagaMedia();
                Vagas.Single(x => x.Id == vaga.Id).Veiculo = veiculo;
            }
            else
            {
                Vaga? vaga = EncontrarVagaGrande();
                Vagas.Single(x => x.Id == vaga.Id).Veiculo = veiculo;
            }
        }
    }
}

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
        //public List<Veiculo> Veiculos { get; set; } = new();
        public double PrecoHora { get; set; }
        public int QtdeVagaPequena { get; set; }
        public int QtdeVagaMedia { get; set; }
        public int QtdeVagaGrande { get; set; }
        public double Caixa { get; set; }
        public List<Aviso> Avisos { get; set; } = new();

        public void CriarVagas()
        {
            for (int i = 0; i < QtdeVagaPequena; i++) { Vaga vagaPequena = new Vaga(Vaga.Type.Pequena); Vagas.Add(vagaPequena); }
            for (int i = 0; i < QtdeVagaMedia; i++) { Vaga vagaMedia = new Vaga(Vaga.Type.Media); Vagas.Add(vagaMedia); }
            for (int i = 0; i < QtdeVagaGrande; i++) { Vaga vagaGrande = new Vaga(Vaga.Type.Grande); Vagas.Add(vagaGrande); }
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

        public IDictionary<int, Veiculo> RetornaDicVeiculosEstacionados()
        {
            List<Veiculo> veiculosEstacionados = Vagas.Where(x => x.Status == State.Ocupada).Select(y => y.Veiculo).ToList();
            IDictionary<int, Veiculo> dicVeiculosEstacionados = new Dictionary<int, Veiculo>();

            for (int i = 0; i < veiculosEstacionados.Count(); i++)
            {
                dicVeiculosEstacionados.Add(i + 1, veiculosEstacionados[i]);
            }

            return dicVeiculosEstacionados;
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
                Vaga? vagaEncontrada = EncontrarVagaPequena();
                Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vagaEncontrada.Id);
                vagaAlterada.Veiculo = veiculo;
                vagaAlterada.Status = State.Ocupada;
            }
            else if (veiculo.Tipo == Veiculo.Type.Carro)
            {
                Vaga? vagaEncontrada = EncontrarVagaMedia();
                Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vagaEncontrada.Id);
                vagaAlterada.Veiculo = veiculo;
                vagaAlterada.Status = State.Ocupada;
            }
            else
            {
                Vaga? vagaEncontrada = EncontrarVagaGrande();
                Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vagaEncontrada.Id);
                vagaAlterada.Veiculo = veiculo;
                vagaAlterada.Status = State.Ocupada;
            }
        }

        public string MostrarMensagens()
        {
            string retornaMensagens = "";
            foreach (var aviso in Avisos)
            {
                if (string.IsNullOrEmpty(retornaMensagens))
                {
                    retornaMensagens = aviso.Mensagem;
                }
                else
                {
                    retornaMensagens += $"\r\n{aviso.Mensagem}";
                }
            }
            return retornaMensagens;
        }

        //public void SaidaVeiculo(Veiculo? veiculo)
        //{
        //    Vagas.Where();
        //}
    }
}

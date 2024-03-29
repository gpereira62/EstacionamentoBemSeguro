﻿using static EstacionamentoBemSeguro.Models.Vaga;

namespace EstacionamentoBemSeguro.Models
{
    internal class Estacionamento
    {
        public List<Vaga> Vagas { get; set; } = new();
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

        public int TotalVagasDisponiveisMoto()
        {
            return Vagas.Where(x => x.Status == State.Disponivel).Where(y => y.Tipo == Vaga.Type.Pequena).Count();
        }

        public int TotalVagasDisponiveisCarro()
        {
            return Vagas.Where(x => x.Status == State.Disponivel).Where(y => y.Tipo == Vaga.Type.Media).Count();
        }

        public int TotalVagasDisponiveisVan()
        {
            return Vagas.Where(x => x.Status == State.Disponivel).Where(y => y.Tipo == Vaga.Type.Grande).Count();
        }

        public int TotalVagasOcupadas()
        {
            return Vagas.Where(x => x.Status == State.Ocupada).Count();
        }

        public bool ExisteVanEstacionada()
        {
            return Vagas.Where(x => x.Veiculo is not null).Where(y => y.Veiculo?.Tipo == Veiculo.Type.Van).Any();
        }

        public bool PodeEstacionarMoto()
        {
            bool podeEstacionar = Vagas.Where(x => x.Status == State.Disponivel).Any();

            if (!podeEstacionar)
                Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar a moto!"));

            return podeEstacionar;
        }

        public bool PodeEstacionarCarro()
        {
            bool podeEstacionar = false;

            podeEstacionar = Vagas.Where(x => x.Tipo == Vaga.Type.Media).Where(y => y.Status == State.Disponivel).Any();

            if (podeEstacionar)
                return podeEstacionar;

            podeEstacionar = Vagas.Where(x => x.Tipo == Vaga.Type.Grande).Where(y => y.Status == State.Disponivel).Any();

            if (!podeEstacionar)
                Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar o carro!"));

            return podeEstacionar;
        }

        public bool PodeEstacionarVan()
        {
            bool podeEstacionar = false;

            podeEstacionar = Vagas.Where(x => x.Tipo == Vaga.Type.Grande).Where(y => y.Status == State.Disponivel).Any();

            if (podeEstacionar)
                return podeEstacionar;

            podeEstacionar = Vagas.Where(x => x.Tipo == Vaga.Type.Media)
                                .Where(y => y.Status == State.Disponivel)
                                .Take(3)
                                .ToList()
                                .Select(x => x != null).Count() == 3;

            if (!podeEstacionar)
                Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar a van!"));

            return podeEstacionar;
        }

        public IDictionary<int, Veiculo> RetornaDicVeiculosEstacionados()
        {
            List<Veiculo?> veiculosEstacionados = Vagas.Where(x => x.Status == State.Ocupada)
                                                        .Select(y => y.Veiculo)
                                                        .Distinct()
                                                        .OrderBy(x => x?.DataHoraEntrada)
                                                        .ToList();

            IDictionary<int, Veiculo> dicVeiculosEstacionados = new Dictionary<int, Veiculo>();

            for (int i = 0; i < veiculosEstacionados.Count(); i++)
            {
                var veiculo = veiculosEstacionados[i];
                if (veiculo is not null)
                {
                    dicVeiculosEstacionados.Add(i + 1, veiculo);
                }
            }

            return dicVeiculosEstacionados;
        }

        public Vaga? EncontrarVagaMoto()
        {
            Vaga? vagaPequena = Vagas.Where(x => x.Tipo == Vaga.Type.Pequena).Where(y => y.Status == State.Disponivel).FirstOrDefault();

            if (vagaPequena is not null)
            {
                return vagaPequena;
            }

            Vaga? vagaMedia = Vagas.Where(x => x.Tipo == Vaga.Type.Media).Where(y => y.Status == State.Disponivel).FirstOrDefault();

            if (vagaMedia is not null)
            {
                return vagaMedia;
            }

            Vaga? vagaGrande = Vagas.Where(x => x.Tipo == Vaga.Type.Grande).Where(y => y.Status == State.Disponivel).FirstOrDefault();

            if (vagaGrande is not null)
            {
                return vagaGrande;
            }

            return null;
        }

        public Vaga? EncontrarVagaCarro()
        {
            Vaga? vagaMedia = Vagas.Where(x => x.Tipo == Vaga.Type.Media).Where(y => y.Status == State.Disponivel).FirstOrDefault();

            if (vagaMedia is not null)
            {
                return vagaMedia;
            }

            Vaga? vagaGrande = Vagas.Where(x => x.Tipo == Vaga.Type.Grande).Where(y => y.Status == State.Disponivel).FirstOrDefault();

            if (vagaGrande is not null)
            {
                return vagaGrande;
            }

            return null;
        }

        public Vaga? EncontrarVagaVan()
        {
            Vaga? vagaGrande = Vagas.Where(x => x.Tipo == Vaga.Type.Grande).Where(y => y.Status == State.Disponivel).FirstOrDefault();

            if (vagaGrande is not null)
            {
                return vagaGrande;
            }

            return null;
        }

        public List<Vaga> EncontrarTresVagasMediasVan()
        {
            return Vagas.Where(x => x.Tipo == Vaga.Type.Media).Where(y => y.Status == State.Disponivel).Take(3).ToList();
        }

        public Vaga? EncontrarVaga(Guid idVeiculo)
        {
            return Vagas.Where(x => x.Veiculo is not null).Where(x => x.Veiculo?.Id == idVeiculo).FirstOrDefault();
        }

        public List<Vaga> EncontrarVagaMediaVan(Guid idVeiculo)
        {
            return Vagas.Where(x => x.Veiculo is not null).Where(x => x.Veiculo?.Id == idVeiculo).ToList();
        }

        public void EstacionarVeiculo(Veiculo veiculo)
        {
            if (veiculo.Tipo == Veiculo.Type.Moto)
            {
                Vaga? vagaEncontrada = EncontrarVagaMoto();

                if (vagaEncontrada is not null)
                {
                    Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vagaEncontrada.Id);
                    vagaAlterada.Veiculo = veiculo;
                    vagaAlterada.Status = State.Ocupada;

                    Avisos.Add(new Aviso("Moto estacionada com sucesso!"));
                }
                else
                {
                    Avisos.Add(new Aviso("Vaga para moto não encontrada!"));
                }
            }
            else if (veiculo.Tipo == Veiculo.Type.Carro)
            {
                Vaga? vagaEncontrada = EncontrarVagaCarro();

                if (vagaEncontrada is not null)
                {
                    Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vagaEncontrada.Id);
                    vagaAlterada.Veiculo = veiculo;
                    vagaAlterada.Status = State.Ocupada;

                    Avisos.Add(new Aviso("Carro estacionado com sucesso!"));
                }
                else
                {
                    Avisos.Add(new Aviso("Vaga para carro não encontrada!"));
                }
            }
            else
            {
                Vaga? vagaEncontrada = EncontrarVagaVan();

                if (vagaEncontrada is not null)
                {
                    Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vagaEncontrada.Id);
                    vagaAlterada.Veiculo = veiculo;
                    vagaAlterada.Status = State.Ocupada;

                    Avisos.Add(new Aviso("Van estacionada com sucesso!"));

                    return;
                }

                List<Vaga> VagasMedias = EncontrarTresVagasMediasVan();

                if (VagasMedias.Select(x => x != null).Count() == 3)
                {
                    veiculo.OcupaVagaMedia = true;

                    foreach (Vaga vaga in VagasMedias)
                    {
                        Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vaga.Id);
                        vagaAlterada.Veiculo = veiculo;
                        vagaAlterada.Status = State.Ocupada;
                    }

                    Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar a van!"));

                    return;
                }
                else
                {
                    Avisos.Add(new Aviso("Vaga para van não encontrada!"));
                }

            }
        }

        public void ExcluirVeiculo(Veiculo veiculo)
        {
            if (veiculo.OcupaVagaMedia)
            {
                List<Vaga> vagasEncontradas = EncontrarVagaMediaVan(veiculo.Id);

                foreach (var vaga in vagasEncontradas)
                {
                    Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vaga.Id);
                    vagaAlterada.Veiculo = null;
                    vagaAlterada.Status = State.Disponivel;
                }

            }
            else
            {
                Vaga? vagaEncontrada = EncontrarVaga(veiculo.Id);

                if (vagaEncontrada != null)
                {
                    Vaga? vagaAlterada = this.Vagas.Single(x => x.Id == vagaEncontrada.Id);
                    vagaAlterada.Veiculo = null;
                    vagaAlterada.Status = State.Disponivel;
                }
            }
        }

        public List<Vaga> ListaVagasVans()
        {
            return Vagas.Where(x => x.Veiculo is not null)
                        .Where(x => x.Veiculo?.Tipo == Veiculo.Type.Van)
                        .ToList()
                        .DistinctBy(x => x.Veiculo)
                        .ToList();
        }

        public bool TemAvisos()
        {
            return Avisos.Any();
        }

        public Tuple<double, TimeSpan> InfoPagamentoSaida(Veiculo veiculo)
        {

            double valorCobrado = new();
            DateTime dataHoraSaida = DateTime.Now;
            TimeSpan tempoEstacionado = dataHoraSaida - veiculo.DataHoraEntrada;

            if (tempoEstacionado.Hours < 1)
            {
                valorCobrado = PrecoHora;
            }
            else
            {
                valorCobrado = int.Parse(tempoEstacionado.Hours.ToString()) * PrecoHora;
            }

            return new Tuple<double, TimeSpan>(valorCobrado, tempoEstacionado);
        }
    }
}

using static EstacionamentoBemSeguro.Models.Vaga;

namespace EstacionamentoBemSeguro.Models
{
    internal class Estacionamento
    {
        private const int VagasPorVan = 3;

        public List<Vaga> Vagas { get; set; } = new();
        public double PrecoHora { get; set; }
        public int QtdeVagaPequena { get; set; }
        public int QtdeVagaMedia { get; set; }
        public int QtdeVagaGrande { get; set; }
        public double Caixa { get; set; }
        public List<Aviso> Avisos { get; set; } = new();

        public void CriarVagas()
        {
            for (int i = 0; i < QtdeVagaPequena; i++) Vagas.Add(new Vaga(Vaga.Type.Pequena));
            for (int i = 0; i < QtdeVagaMedia; i++) Vagas.Add(new Vaga(Vaga.Type.Media));
            for (int i = 0; i < QtdeVagaGrande; i++) Vagas.Add(new Vaga(Vaga.Type.Grande));
        }

        public int TotalVagas() => Vagas.Count;

        public int TotalVagasDisponiveis() => Vagas.Count(x => x.Status == State.Disponivel);

        public int TotalVagasOcupadas() => Vagas.Count(x => x.Status == State.Ocupada);

        public int TotalVagasDisponiveisPorTipo(Vaga.Type tipo)
            => Vagas.Count(x => x.Status == State.Disponivel && x.Tipo == tipo);

        public int TotalVagasDisponiveisMoto() => TotalVagasDisponiveisPorTipo(Vaga.Type.Pequena);
        public int TotalVagasDisponiveisCarro() => TotalVagasDisponiveisPorTipo(Vaga.Type.Media);
        public int TotalVagasDisponiveisVan() => TotalVagasDisponiveisPorTipo(Vaga.Type.Grande);

        public bool ExisteVanEstacionada()
            => Vagas.Any(x => x.Veiculo?.Tipo == Veiculo.Type.Van);

        public bool PodeEstacionarMoto()
        {
            bool pode = Vagas.Any(x => x.Status == State.Disponivel);
            if (!pode)
                Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar a moto!"));
            return pode;
        }

        public bool PodeEstacionarCarro()
        {
            bool pode = Vagas.Any(x => x.Status == State.Disponivel &&
                                       (x.Tipo == Vaga.Type.Media || x.Tipo == Vaga.Type.Grande));
            if (!pode)
                Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar o carro!"));
            return pode;
        }

        public bool PodeEstacionarVan()
        {
            bool temGrande = Vagas.Any(x => x.Tipo == Vaga.Type.Grande && x.Status == State.Disponivel);
            bool temTresMedias = Vagas.Count(x => x.Tipo == Vaga.Type.Media && x.Status == State.Disponivel) >= VagasPorVan;

            bool pode = temGrande || temTresMedias;
            if (!pode)
                Avisos.Add(new Aviso("Não há espaço no estacionamento para estacionar a van!"));
            return pode;
        }

        public IDictionary<int, Veiculo> RetornaDicVeiculosEstacionados()
        {
            var veiculos = Vagas
                .Where(x => x.Status == State.Ocupada && x.Veiculo is not null)
                .Select(x => x.Veiculo!)
                .Distinct()
                .OrderBy(x => x.DataHoraEntrada)
                .ToList();

            var dic = new Dictionary<int, Veiculo>();
            for (int i = 0; i < veiculos.Count; i++)
                dic.Add(i + 1, veiculos[i]);

            return dic;
        }

        private Vaga? PrimeiraVagaDisponivel(params Vaga.Type[] prioridades)
            => prioridades
                .Select(t => Vagas.FirstOrDefault(v => v.Tipo == t && v.Status == State.Disponivel))
                .FirstOrDefault(v => v is not null);

        private Vaga? EncontrarVagaMoto() => PrimeiraVagaDisponivel(Vaga.Type.Pequena, Vaga.Type.Media, Vaga.Type.Grande);
        private Vaga? EncontrarVagaCarro() => PrimeiraVagaDisponivel(Vaga.Type.Media, Vaga.Type.Grande);
        private Vaga? EncontrarVagaVan() => PrimeiraVagaDisponivel(Vaga.Type.Grande);

        private List<Vaga> EncontrarTresVagasMediasVan()
            => Vagas.Where(x => x.Tipo == Vaga.Type.Media && x.Status == State.Disponivel)
                    .Take(VagasPorVan)
                    .ToList();

        public Vaga? EncontrarVaga(Guid idVeiculo)
            => Vagas.FirstOrDefault(x => x.Veiculo?.Id == idVeiculo);

        public List<Vaga> EncontrarVagaMediaVan(Guid idVeiculo)
            => Vagas.Where(x => x.Veiculo?.Id == idVeiculo).ToList();

        private void OcuparVaga(Vaga vaga, Veiculo veiculo)
        {
            var vagaAlterada = Vagas.Single(x => x.Id == vaga.Id);
            vagaAlterada.Veiculo = veiculo;
            vagaAlterada.Status = State.Ocupada;
        }

        private void LiberarVaga(Vaga vaga)
        {
            var vagaAlterada = Vagas.Single(x => x.Id == vaga.Id);
            vagaAlterada.Veiculo = null;
            vagaAlterada.Status = State.Disponivel;
        }

        public void EstacionarVeiculo(Veiculo veiculo)
        {
            if (veiculo.Tipo == Veiculo.Type.Moto)
            {
                Vaga? vaga = EncontrarVagaMoto();
                if (vaga is not null)
                {
                    OcuparVaga(vaga, veiculo);
                    Avisos.Add(new Aviso("Moto estacionada com sucesso!"));
                }
                else
                {
                    Avisos.Add(new Aviso("Vaga para moto não encontrada!"));
                }
            }
            else if (veiculo.Tipo == Veiculo.Type.Carro)
            {
                Vaga? vaga = EncontrarVagaCarro();
                if (vaga is not null)
                {
                    OcuparVaga(vaga, veiculo);
                    Avisos.Add(new Aviso("Carro estacionado com sucesso!"));
                }
                else
                {
                    Avisos.Add(new Aviso("Vaga para carro não encontrada!"));
                }
            }
            else // Van
            {
                Vaga? vagaGrande = EncontrarVagaVan();
                if (vagaGrande is not null)
                {
                    OcuparVaga(vagaGrande, veiculo);
                    Avisos.Add(new Aviso("Van estacionada com sucesso!"));
                    return;
                }

                List<Vaga> vagasMedias = EncontrarTresVagasMediasVan();
                if (vagasMedias.Count == VagasPorVan)
                {
                    veiculo.OcupaVagaMedia = true;
                    foreach (var vaga in vagasMedias)
                        OcuparVaga(vaga, veiculo);
                    Avisos.Add(new Aviso("Van estacionada com sucesso em 3 vagas médias!"));
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
                foreach (var vaga in EncontrarVagaMediaVan(veiculo.Id))
                    LiberarVaga(vaga);
            }
            else
            {
                Vaga? vaga = EncontrarVaga(veiculo.Id);
                if (vaga is not null)
                    LiberarVaga(vaga);
            }
        }

        public List<Vaga> ListaVagasVans()
            => Vagas
                .Where(x => x.Veiculo?.Tipo == Veiculo.Type.Van)
                .DistinctBy(x => x.Veiculo)
                .ToList();

        public bool TemAvisos() => Avisos.Any();

        public Tuple<double, TimeSpan> InfoPagamentoSaida(Veiculo veiculo)
        {
            DateTime dataHoraSaida = DateTime.Now;
            TimeSpan tempoEstacionado = dataHoraSaida - veiculo.DataHoraEntrada;

            int horas = Math.Max(1, (int)tempoEstacionado.TotalHours);
            double valorCobrado = horas * PrecoHora;

            return new Tuple<double, TimeSpan>(valorCobrado, tempoEstacionado);
        }
    }
}

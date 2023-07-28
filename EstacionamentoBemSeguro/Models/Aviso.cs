namespace EstacionamentoBemSeguro.Models
{
    internal class Aviso
    {
        public string Mensagem { get; set; } = "";

        public Aviso(string mensagem)
        {
            this.Mensagem = mensagem;
        }

        public static Estacionamento ChecarAvisos(Estacionamento estacionamento)
        {
            if (estacionamento.TotalVagasDisponiveis() == 0)
            {
                estacionamento.Avisos.Add(new Aviso("Estacionamento cheio!"));
            }
            else
            {
                if (estacionamento.TotalVagasDisponiveisMoto() == 0 && estacionamento.QtdeVagaPequena > 0)
                {
                    estacionamento.Avisos.Add(new Aviso("Todas as vagas pequenas para motos foram ocupadas!"));
                }

                if (estacionamento.TotalVagasDisponiveisCarro() == 0 && estacionamento.QtdeVagaMedia > 0)
                {
                    estacionamento.Avisos.Add(new Aviso("Todas as vagas médias para carros foram ocupadas!"));
                }

                if (estacionamento.TotalVagasDisponiveisVan() == 0 && estacionamento.QtdeVagaGrande > 0)
                {
                    estacionamento.Avisos.Add(new Aviso("Todas as vagas grandes para vans foram ocupadas!"));
                }
            }

            if (estacionamento.TotalVagasOcupadas() == 0)
            {
                estacionamento.Avisos.Add(new Aviso("Estacionamento vazio!"));
            }

            return estacionamento;
        }
    }

}

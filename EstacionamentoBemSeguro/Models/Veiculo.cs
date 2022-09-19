using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoBemSeguro.Models
{
    internal class Veiculo
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = "";
        public string Placa { get; set; } = "";
        public Type Tipo { get; set; }
        public DateTime DataHoraEntrada { get; set; }

        public enum Type
        {
            Carro = 0,
            Moto = 1,
            Van = 2
        }
        public Veiculo(Type tipo)
        {
            this.Tipo = tipo;
            this.Id = Guid.NewGuid();
            this.DataHoraEntrada = DateTime.Now;
        }

        public Veiculo(string nome, string placa, Type tipo)
        {
            this.Nome = nome;
            this.Placa = placa;
            this.Tipo = tipo;
            this.Id = Guid.NewGuid();
            this.DataHoraEntrada = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Veículo: {Nome} | Placa: {Placa} | Tipo: {Tipo.ToString()}";
        }

    }
}

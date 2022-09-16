using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoBemSeguro.Models
{
    internal class Vaga
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public State Status { get; set; } = State.Disponivel;
        public Veiculo? Veiculo { get; set; } = null;

        public enum State
        {
            Disponivel = 0,
            Ocupada = 1,
        }

        public Type Tipo { get; set; }

        public enum Type
        {
            Pequena = 0,
            Media = 1,
            Grande = 2,
        }

        public Vaga(Type tipo)
        {
            this.Tipo = tipo;
        }

    }
}

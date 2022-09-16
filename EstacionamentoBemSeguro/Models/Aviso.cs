using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoBemSeguro.Models
{
    internal class Aviso
    {
        public string Mensagem { get; set; } = "";

        public Aviso(string mensagem)
        {
            this.Mensagem = mensagem;
        }
    }


}

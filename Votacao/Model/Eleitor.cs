using System;
using System.Collections.Generic;
using System.Text;

namespace Votacao.Model
{
    internal class Eleitor
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }

        public Eleitor(long id, string nome, string titulo)
        {
            Id = id;
            Nome = nome;
            Titulo = titulo;
        }
    }
}

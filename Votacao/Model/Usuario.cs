using System;
using System.Collections.Generic;
using System.Text;

namespace Votacao.Model
{
    internal class Usuario
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public bool Auditor { get; set; }

        public Usuario(long id, string nome, string login, bool auditor)
        {
            Id = id;
            Nome = nome;
            Login = login;
            Auditor = auditor;
        }
    }
}

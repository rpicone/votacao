using System;
using System.Collections.Generic;
using System.Text;

namespace Votacao.Model
{
    internal class UsuarioNovo
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string SenhaHash { get; set; }
        public string Salt { get; set; }
        public bool Auditor { get; set; }

        public UsuarioNovo(string nome, string login, bool auditor)
        {
            Nome = nome;
            Login = login;
            Auditor = auditor;
        }
    }
}

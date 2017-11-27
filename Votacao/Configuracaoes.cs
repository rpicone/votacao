using System;
using System.Collections.Generic;
using System.Text;

namespace Votacao
{
    internal class Configuracaoes
    {
        public static readonly string ConnectionString;

        static Configuracaoes()
        {
            ConnectionString = "Database=votacao;Server=localhost;Port=5432;UserId=votacao;Password=votacao;";
        }

    }
}

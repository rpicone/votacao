using System;

namespace Votacao
{
    class Program
    {
        static void Main(string[] args)
        {
            var aplicacao = new Aplicacao();
            aplicacao.Autenticar();
            aplicacao.Iniciar();
            aplicacao.Encerrar();
        }
    }
}

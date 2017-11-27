using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Votacao.Model;
using Votacao.Service;

namespace Votacao
{
    static class Votacao
    {
        public static void Votar()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("DIGITE O NÚMERO DO TÍTULO DE ELEITOR E PRESSIONE ENTER OU APENAS ENTER PARA VOLTAR.");
                var tituloInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(tituloInput))
                {
                    break;
                }

                var eleitor = EleitorService.ConsultarPorTitulo(tituloInput);
                if (eleitor == null)
                {
                    Console.WriteLine("ELEITOR NÃO ENCONTRADO COM ESTE TÍTULO! PRESSIONE QUALQUER TECLA PARA CONTINUAR.");
                    Console.ReadKey(true);
                    continue;
                }
                Console.WriteLine($"Nome do Eleitor: { eleitor.Nome }");

                if (VotoService.EleitorJaVotou(eleitor.Titulo))
                {
                    Console.WriteLine("ESTE ELEITOR JÁ VOTOU! PRESSIONE QUALQUER TECLA PARA CONTINUAR.");
                    Console.ReadKey(true);
                    continue;
                }

                var prefeito = ColherVotoPrefeito();
                var vereador = ColherVotoVereador();
                VotoService.RegistrarVoto(eleitor, prefeito, vereador);
                Console.WriteLine("PARABÉNS, SUA VOTAÇÃO FOI CONLUÍDA! PRESSIONE QUALQUER TECLA PARA CONTINUAR.");
                Console.ReadKey(true);
            }
        }

        private static Candidato ColherVotoPrefeito()
        {
            Console.WriteLine("");
            Console.WriteLine("VOTO PARA PREFEITO");
            Console.WriteLine("==================");
            while (true)
            {
                Console.WriteLine("Instruções: Digite o número do candidato, B para branco ou N para nulo e pressione ENTER");
                var voto = Console.ReadLine().Trim();
                if (voto != "B" && voto != "N")
                {
                    var candidato = CandidatoService.ConsultarPorNumero(voto, 1);
                    if (candidato == null)
                    {
                        Console.WriteLine("CANDIDATO NÃO ENCONTRADO COM ESTE NÚMERO. TENTE NOVAMENTE.");
                        continue;
                    }
                    Console.WriteLine($"Nome do Candidato: { candidato.Nome }");
                    var confirmacao = Aplicacao.Confirmacao();
                    if (!confirmacao)
                    {
                        continue;
                    }
                    return candidato;
                }
            }
        }

        private static Candidato ColherVotoVereador()
        {
            Console.WriteLine("");
            Console.WriteLine("VOTO PARA VEREADOR");
            Console.WriteLine("==================");
            while (true)
            {
                Console.WriteLine("Instruções: Digite o número do candidato, B para branco ou N para nulo e pressione ENTER");
                var voto = Console.ReadLine().Trim();
                if (voto != "B" && voto != "N")
                {
                    var candidato = CandidatoService.ConsultarPorNumero(voto, 2);
                    if (candidato == null)
                    {
                        Console.WriteLine("CANDIDATO NÃO ENCONTRADO COM ESTE NÚMERO. TENTE NOVAMENTE.");
                        continue;
                    }
                    Console.WriteLine($"Nome do Candidato: { candidato.Nome }");
                    var confirmacao = Aplicacao.Confirmacao();
                    if (!confirmacao)
                    {
                        continue;
                    }
                    return candidato;
                }
            }
        }

        public static void ExibirApuracao()
        {
            var apuracao = VotoService.RelatorioApuracao();

            Console.Clear();
            Console.WriteLine("RELATÓRIO DE APURACAÇÃO DE VOTAÇÃO ENCERRADA");
            Console.WriteLine("============================================");
            Console.WriteLine("");
            Console.WriteLine("PREFEITO");
            Console.WriteLine("--------------------------------------------");
            var candidatosPrefeito = apuracao.Where(c => c.CargoId == 1);
            int ordem = 0;
            foreach (var candidato in candidatosPrefeito)
            {
                Console.WriteLine($"{(++ordem).ToString("D2")} - {candidato.CandidatoNome} ({candidato.CandidatoNumero}): {candidato.Votos} votos");
            }

            Console.WriteLine("");
            Console.WriteLine("VEREADOR");
            Console.WriteLine("--------------------------------------------");
            var candidatosVereador = apuracao.Where(c => c.CargoId == 2);
            ordem = 0;
            foreach (var candidato in candidatosVereador)
            {
                Console.WriteLine($"{(++ordem).ToString("D2")} - {candidato.CandidatoNome} ({candidato.CandidatoNumero}): {candidato.Votos} votos");
            }

            Console.WriteLine("");
            Console.WriteLine("FIM DO RELATÓRIO! PRESSIONE QUALQUER TECLA PARA CONTINUAR.");
            Console.ReadKey(true);
        }

        public static void EncerrarVotacao()
        {
            Console.Clear();
            Console.WriteLine("================================================================");
            Console.WriteLine("VOCÊ IRÁ ENCERRARA A VOTAÇÃO. ESSA AÇÃO NÃO PODERÁ SER DESFEITA.");
            Console.WriteLine("================================================================");

            if (Aplicacao.Confirmacao())
            {
                VotoService.EncerrarVotacao();
            }

            return;
        }
    }
}

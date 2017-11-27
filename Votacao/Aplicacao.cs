using System;
using System.Collections.Generic;
using System.Text;
using Votacao.Model;
using Votacao.Service;

namespace Votacao
{
    class Aplicacao
    {
        public static Usuario UsuarioAutenticado { get; set; }

        public bool Autenticar()
        {
            return GestaoUsuarios.Autenticar();
        }

        public void Iniciar()
        {
            Console.WriteLine("SISTEMA DE VOTAÇÃO ELETRÔNICO");
            Console.WriteLine("=============================");
            MenuPrincipal();
        }

        public void Encerrar()
        {
            Console.WriteLine("PRESSIONE QUALQUER TECLA PARA ENCERRAR O SISTEMA");
            Console.ReadKey(true);
        }

        public void MenuPrincipal()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("MENU PRINCIPAL");
                Console.WriteLine("--------------");
                Console.WriteLine("1: Votação");
                if (UsuarioAutenticado.Auditor)
                {
                    Console.WriteLine("2: Gerenciar Usuários");
                }
                Console.WriteLine("9: Sair");
                var k = Console.ReadKey(true);
                switch (k.KeyChar)
                {
                    case '1':
                        MenuVotacao();
                        break;
                    case '2':
                        if (UsuarioAutenticado.Auditor)
                        {
                            MenuGerenciarUsuarios();
                        }
                        break;
                    case '9':
                        return;
                    default:
                        OpcaoInvalida();
                        break;
                }
            }
        }

        public void MenuGerenciarUsuarios()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("USUÁRIOS");
                Console.WriteLine("--------");
                Console.WriteLine("1: Cadastrar");
                Console.WriteLine("2: Listar");
                Console.WriteLine("3: Excluir");
                Console.WriteLine("9: Voltar");
                var k = Console.ReadKey(true);
                switch (k.KeyChar)
                {
                    case '1':
                        GestaoUsuarios.Inserir();
                        break;
                    case '2':
                        GestaoUsuarios.Listar();
                        break;
                    case '3':
                        GestaoUsuarios.Excluir();
                        break;
                    case '9':
                        return;
                    default:
                        OpcaoInvalida();
                        break;
                }
            }
        }

        public void MenuVotacao()
        {
            while (true)
            {
                var votacaoAberta = VotoService.VotacaoAberta();

                if (votacaoAberta)
                {
                    Console.Clear();
                    Console.WriteLine("VOTAÇÃO EM ANDAMENTO");
                    Console.WriteLine("--------------------");
                    Console.WriteLine("1: Votar");
                    Console.WriteLine("2: Encerrar Votação");
                    Console.WriteLine("9: Voltar");
                    var k = Console.ReadKey(true);
                    switch (k.KeyChar)
                    {
                        case '1':
                            Votacao.Votar();
                            break;
                        case '2':
                            Votacao.EncerrarVotacao();
                            break;
                        case '9':
                            return;
                        default:
                            OpcaoInvalida();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("VOTAÇÃO");
                    Console.WriteLine("-------");
                    Console.WriteLine("1: Iniciar Nova Votação (apaga os dados da votação anterior)");
                    if (UsuarioAutenticado.Auditor)
                    {
                        Console.WriteLine("2: Exibir Relatório de Apuração");
                    }
                    Console.WriteLine("9: Voltar");
                    var k = Console.ReadKey(true);
                    switch (k.KeyChar)
                    {
                        case '1':
                            VotoService.IniciarVotacao();
                            break;
                        case '2':
                            if (UsuarioAutenticado.Auditor)
                            {
                                Votacao.ExibirApuracao();
                            }
                            break;
                        case '9':
                            return;
                        default:
                            OpcaoInvalida();
                            break;
                    }
                }
            }
        }

        public void OpcaoInvalida()
        {
            Console.WriteLine("OPÇÃO INVÁLIDA!");
        }

        public static bool Confirmacao()
        {
            Console.WriteLine("Confirma? (S/N)");
            while (true)
            {
                var confirmacao = Console.ReadKey(true);
                switch (confirmacao.Key)
                {
                    case ConsoleKey.S:
                        return true;
                    case ConsoleKey.N:
                        return false;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the console password.
        /// </summary>
        /// <returns></returns>
        public static string GetConsolePassword()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        sb.Length--;
                    }

                    continue;
                }

                Console.Write('*');
                sb.Append(cki.KeyChar);
            }

            return sb.ToString();
        }
    }
}

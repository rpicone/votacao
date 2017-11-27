using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Votacao.Model;
using Votacao.Service;

namespace Votacao
{
    static class GestaoUsuarios
    {
        public static void Listar()
        {
            var usuarios = UsuarioService.Listar();

            Console.Clear();
            Console.WriteLine("LISTAGEM DE USUÁRIOS");
            Console.WriteLine("====================");
            Console.WriteLine("");
            foreach (var usuario in usuarios)
            {
                Console.WriteLine($"Nome: {usuario.Nome}, Login: ({usuario.Login}), Auditor: {usuario.Auditor};");
            }

            Console.WriteLine("");
            Console.WriteLine("FIM DO RELATÓRIO! PRESSIONE QUALQUER TECLA PARA CONTINUAR.");
            Console.ReadKey(true);
        }

        public static bool Autenticar()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("LOGIN:");
                var loginInput = Console.ReadLine().Trim();

                Console.WriteLine("SENHA:");
                var senhaInput = Aplicacao.GetConsolePassword();

                var dadosSenha = UsuarioService.ConsultaSenha(loginInput);
                if (dadosSenha == null ||
                    !VerificaSenha(senhaInput, dadosSenha[0], dadosSenha[1]))
                {
                    Thread.Sleep(2000);
                    Console.WriteLine("USUÁRIO/SENHA INVÁLIDOS. PRESSIONE QUALQUER TECLA PARA TENTAR NOVAMENTE.");
                    Console.ReadKey(true);
                    continue;
                }

                var usuario = UsuarioService.ConsultaPorLogin(loginInput);
                if (usuario == null)
                {
                    Console.WriteLine("NÃO FOI POSSÍVEL OBTER OS DADOS DO USUÁRIO. PRESSIONE QUALQUER TECLA PARA TENTAR NOVAMENTE.");
                    Console.ReadKey(true);
                    continue;
                }

                Aplicacao.UsuarioAutenticado = usuario;
                return true;
            }
        }

        public static void Inserir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("LOGIN:");
                var loginInput = Console.ReadLine().Trim();

                var usuario = UsuarioService.ConsultaPorLogin(loginInput);
                if (usuario != null)
                {
                    Console.WriteLine("JÁ EXISTE UM USUÁRIO CADASTRADO COM ESTE LOGIN. PRESSIONE QUALQUER TECLA PARA VOLTAR.");
                    Console.ReadKey(true);
                    break;
                }

                Console.WriteLine("NOME:");
                var nomeInput = Console.ReadLine().Trim();
                Console.WriteLine("USUÁRIO AUDITOR? (S/N)");
                bool auditor;
                while (true)
                {
                    var auditorInput = Console.ReadKey(true);
                    if (auditorInput.Key == ConsoleKey.S)
                    {
                        auditor = true;
                        Console.WriteLine("S");
                        break;
                    }
                    if (auditorInput.Key == ConsoleKey.N)
                    {
                        auditor = false;
                        Console.WriteLine("N");
                        break;
                    }
                }

                string senhaInput;

                while (true)
                {
                    Console.WriteLine("SENHA:");
                    senhaInput = Aplicacao.GetConsolePassword();
                    Console.WriteLine("CONFIRMAÇÃO DA SENHA:");
                    var senhaConfirmacaoInput = Aplicacao.GetConsolePassword();
                    if (!senhaInput.Equals(senhaConfirmacaoInput))
                    {
                        Console.WriteLine("A SENHA E SUA CONFIRMAÇÃO DEVEM SER IGUAIS. PRESSIONE QUALQUER TECLA PARA TENTAR NOVAMENTE.");
                        Console.ReadKey(true);
                        continue;
                    }
                    break;
                }

                if (string.IsNullOrWhiteSpace(loginInput) ||
                    string.IsNullOrWhiteSpace(nomeInput) ||
                    string.IsNullOrWhiteSpace(senhaInput))
                {
                    Console.WriteLine("TODAS AS INFORMAÇÕES SÃO OBRIGATÓRIAS. PRESSIONE QUALQUER TECLA PARA VOLTAR.");
                    Console.ReadKey(true);
                    break;
                }

                var novoUsuario = new UsuarioNovo(nomeInput, loginInput, auditor);
                HashSenha(novoUsuario, senhaInput);
                UsuarioService.Inserir(novoUsuario);
                Console.WriteLine("");
                Console.WriteLine("PRESSIONE QUALQUER TECLA PARA VOLTAR.");
                Console.ReadKey(true);
                break;
            }
        }

        public static void Excluir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("DIGITE O LOGIN DO USUÁRIO QUE DESEJA EXCLUIR E PRESSIONE ENTER.");
                var loginInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(loginInput))
                {
                    break;
                }

                var usuario = UsuarioService.ConsultaPorLogin(loginInput);
                if (usuario == null)
                {
                    Console.WriteLine("USUARIO NÃO ENCONTRADO COM ESTE LOGIN! PRESSIONE QUALQUER TECLA PARA CONTINUAR.");
                    Console.ReadKey(true);
                    continue;
                }
                Console.WriteLine($"Nome do Usuário: { usuario.Nome }");
                Console.WriteLine("O USUÁRIO SERÁ EXCLUÍDO E NÃO PODERÁ MAIS SER RECUPERADO!");
                if (Aplicacao.Confirmacao())
                {
                    UsuarioService.Excluir(usuario);
                    Console.WriteLine("USUÁRIO EXCLUÍDO COM SUCESSO! PRESSIONE QUALQUER TECLA PARA CONTINUAR.");
                    Console.ReadKey(true);
                }
            }
        }

        private static void HashSenha(UsuarioNovo usuario, string senha)
        {
            Guid saltGuid = Guid.NewGuid();
            byte[] saltBytes = saltGuid.ToByteArray();
            
            // Derivando uma chave de 256bits utilizanod HMACSHA1 e dez mil iterações
            string hashSenha = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: senha,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            usuario.SenhaHash = hashSenha;
            usuario.Salt = saltGuid.ToString();
        }

        private static bool VerificaSenha(string senha, string senhaHashArmazenada, string saltArmazenado)
        {
            Guid saltGuid = new Guid(saltArmazenado);
            byte[] saltBytes = saltGuid.ToByteArray();

            // Derivando uma chave de 256bits utilizanod HMACSHA1 e dez mil iterações
            string hashSenha = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: senha,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashSenha.Equals(senhaHashArmazenada);
        }
    }
}

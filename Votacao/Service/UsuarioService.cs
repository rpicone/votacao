using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using Votacao.Model;

namespace Votacao.Service
{
    internal class UsuarioService
    {
        private static string queryUsuarios = "SELECT u.id, u.nome, u.login, u.auditor FROM usuario u ORDER BY u.nome";
        public static List<Usuario> Listar()
        {
            var apuracao = new List<Usuario>();
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryUsuarios, conexao))
                {
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        apuracao.Add(new Usuario((long)reader["id"], (string)reader["nome"], (string)reader["login"], (bool)reader["auditor"]));
                    }
                }
            }
            return apuracao;
        }

        private static string queryConsultaPorLogin = "SELECT u.id, u.nome, u.login, u.auditor FROM usuario u WHERE u.login = @login";
        public static Usuario ConsultaPorLogin(string login)
        {
            var apuracao = new List<Usuario>();
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryConsultaPorLogin, conexao))
                {
                    comando.Parameters.AddWithValue("login", login);
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Usuario((long)reader["id"], (string)reader["nome"], (string)reader["login"], (bool)reader["auditor"]);
                    } else
                    {
                        return null;
                    }
                }
            }
        }

        private static string querySenha = "SELECT u.senhahash, u.salt FROM usuario u WHERE u.login = @login";
        public static string[] ConsultaSenha(string login)
        {
            var apuracao = new List<Usuario>();
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(querySenha, conexao))
                {
                    comando.Parameters.AddWithValue("login", login);
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        return new string[] { (string)reader["senhahash"], (string)reader["salt"] };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        private static string queryInserir = "INSERT INTO usuario (id, nome, login, auditor, senhahash, salt) VALUES (NEXTVAL('usuario_seq'), @nome, @login, @auditor, @senhahash, @salt)";
        public static void Inserir(UsuarioNovo usuario)
        {
            var apuracao = new List<Usuario>();
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryInserir, conexao))
                {
                    comando.Parameters.AddWithValue("nome", usuario.Nome);
                    comando.Parameters.AddWithValue("login", usuario.Login);
                    comando.Parameters.AddWithValue("auditor", usuario.Auditor);
                    comando.Parameters.AddWithValue("senhahash", usuario.SenhaHash);
                    comando.Parameters.AddWithValue("salt", usuario.Salt);
                    var reader = comando.ExecuteNonQuery();
                }
            }
        }

        private static string queryExcluir = "DELETE FROM usuario WHERE id = @id";
        public static void Excluir(Usuario usuario)
        {
            var apuracao = new List<Usuario>();
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryExcluir, conexao))
                {
                    comando.Parameters.AddWithValue("id", usuario.Id);
                    var reader = comando.ExecuteNonQuery();
                }
            }
        }
    }
}

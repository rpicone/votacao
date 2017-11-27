using System.Data;
using Npgsql;
using Votacao.Model;
using System;
using System.Collections.Generic;

namespace Votacao.Service
{
    internal class VotoService
    {
        private static string queryInicarVotacao = "INSERT INTO votacao (id, inicio) VALUES (1, 'now'::timestamp); DELETE FROM presenca; UPDATE candidato SET votos = 0;";
        public static void IniciarVotacao()
        {
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryInicarVotacao, conexao)) {
                    comando.ExecuteNonQuery();
                }
            }
        }

        private static string queryEncerrarVotacao = "DELETE FROM votacao;";
        public static void EncerrarVotacao()
        {
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryEncerrarVotacao, conexao))
                {
                    comando.ExecuteNonQuery();
                }
            }
        }

        private static string queryRelatorioApuracaoVotacao = "SELECT c.cargoid, c.numero, c.nome, c.votos FROM candidato c ORDER BY c.votos DESC";
        public static List<Apuracao> RelatorioApuracao()
        {
            var apuracao = new List<Apuracao>();
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryRelatorioApuracaoVotacao, conexao))
                {
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        apuracao.Add(new Apuracao((long)reader["cargoid"], (string)reader["numero"], (string)reader["nome"], (int)reader["votos"]));
                    }
                }
            }
            return apuracao;
        }

        private static string queryVotacaoAberta = "SELECT id FROM votacao";
        public static bool VotacaoAberta()
        {
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryVotacaoAberta, conexao))
                {
                    var reader = comando.ExecuteReader();
                    return reader.Read();
                }
            }
        }

        private static string queryConsultarPresenca = "SELECT p.datahora FROM presenca p LEFT JOIN eleitor e ON e.id = p.eleitorid WHERE e.titulo = @titulo";
        public static bool EleitorJaVotou(string titulo)
        {
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryConsultarPresenca, conexao))
                {
                    comando.Parameters.AddWithValue("titulo", titulo);
                    var reader = comando.ExecuteReader();
                    return reader.Read();
                }
            }
        }

        private static string queryInserirPresenca = "INSERT INTO presenca (eleitorid) VALUES (@eleitorid)";
        private static string queryIncrementarVotoNoCandidato = "UPDATE candidato SET votos = votos + 1 WHERE id = @id";
        public static void RegistrarVoto(Eleitor eleitor, Candidato prefeito, Candidato vereador)
        {
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                var transacao = conexao.BeginTransaction();
                try
                {
                    // Registrando Presença
                    using (var comando = new NpgsqlCommand(queryInserirPresenca, conexao))
                    {
                        comando.Parameters.AddWithValue("eleitorid", eleitor.Id);
                        comando.ExecuteNonQuery();
                    }

                    // Contabilizando voto no prefeito
                    using (var comando = new NpgsqlCommand(queryIncrementarVotoNoCandidato, conexao))
                    {
                        comando.Parameters.AddWithValue("id", prefeito.Id);
                        comando.ExecuteNonQuery();
                    }

                    // Contabilizando voto no vereador
                    using (var comando = new NpgsqlCommand(queryIncrementarVotoNoCandidato, conexao))
                    {
                        comando.Parameters.AddWithValue("id", vereador.Id);
                        comando.ExecuteNonQuery();
                    }

                    transacao.Commit();
                }
                catch(Exception exception)
                {
                    transacao.Rollback();
                    throw exception;
                }
            }
        }
    }
}

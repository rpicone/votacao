using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using Votacao.Model;

namespace Votacao.Service
{
    internal class CandidatoService
    {
        private static string queryConsultaPorTitulo = "SELECT id, numero, nome, partidoid, cargoid FROM candidato WHERE numero = @numero AND cargoid = @cargoid";

        public static Candidato ConsultarPorNumero(string numero, long cargoId)
        {
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryConsultaPorTitulo, conexao))
                {
                    comando.Parameters.AddWithValue("numero", numero);
                    comando.Parameters.AddWithValue("cargoid", cargoId);
                    var reader = comando.ExecuteReader();
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new Candidato((long)reader["id"], (string)reader["numero"], (long)reader["cargoid"],
                        (string)reader["nome"], (long)reader["partidoid"]);
                }
            }
        }
    }
}

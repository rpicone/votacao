using System.Data;
using Npgsql;
using Votacao.Model;

namespace Votacao.Service
{
    internal class EleitorService
    {
        private static string queryConsultaPorTitulo = "SELECT id, titulo, nome FROM eleitor WHERE titulo = @titulo";

        public static Eleitor ConsultarPorTitulo(string titulo)
        {
            using (var conexao = new NpgsqlConnection(Configuracaoes.ConnectionString))
            {
                conexao.Open();
                using (var comando = new NpgsqlCommand(queryConsultaPorTitulo, conexao))
                {
                    comando.Parameters.AddWithValue("titulo", titulo);
                    var reader = comando.ExecuteReader();
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new Eleitor((long)reader["id"], (string)reader["nome"], (string)reader["titulo"]);
                }
            }
        }
    }
}

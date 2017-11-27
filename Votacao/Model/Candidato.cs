using System;
using System.Collections.Generic;
using System.Text;

namespace Votacao.Model
{
    internal class Candidato
    {
        public long Id { get; set; }
        public string Numero { get; set; }
        public long CargoId { get; set; }
        public string Nome { get; set; }
        public long PartidoId { get; set; }

        public Candidato(long id, string numero, long cargoId, string nome, long partidoId)
        {
            Id = id;
            Numero = numero;
            CargoId = cargoId;
            Nome = nome;
            PartidoId = partidoId;
        }
    }
}

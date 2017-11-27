using System;
using System.Collections.Generic;
using System.Text;

namespace Votacao.Model
{
    internal class Apuracao
    {
        public long CargoId { get; set; }
        public string CandidatoNumero { get; set; }
        public string CandidatoNome { get; set; }
        public int Votos { get; set; }

        public Apuracao(long cargoId, string candidatoNumero, string candidatoNome, int votos)
        {
            CargoId = cargoId;
            CandidatoNumero = candidatoNumero;
            CandidatoNome = candidatoNome;
            Votos = votos;
        }
    }
}

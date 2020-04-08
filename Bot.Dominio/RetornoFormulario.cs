using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot.Dominio
{
    public class IdadeUsuario
    {
        public string Idade { get; set; }
    }

    public class SexoUsuario
    {
        public string Sexo { get; set; }
    }

    public class Doencas
    {
        public bool Diabetes { get; set; }
        public bool RenalCronica { get; set; }
        public bool RespiratoriaCronica { get; set; }
        public bool PressaoAlta { get; set; }
        public bool Nenhuma { get; set; }
    }

    public class Sintomas
    {
        public bool GargantaCocando { get; set; }
        public bool DorGarganta { get; set; }
        public bool NarizEntupido { get; set; }
        public bool NarizEscorrendo { get; set; }
        public bool Espirros { get; set; }
        public bool OlhosLacrimejando { get; set; }
        public bool OlhosInchados { get; set; }
        public bool Nenhuma { get; set; }
    }


    public class OutrosSintomas
    {
        public string DorMuscular { get; set; }
        public string DorArticulacoes { get; set; }
        public string TosseSeca { get; set; }
        public string Febre { get; set; }
        public string Diarreia { get; set; }
        public string RespiracaoCurtaRaza { get; set; }
        public string SensacaoDesmaio { get; set; }
        public string Enjoo { get; set; }
        public string Nenhum { get; set; }
    }


    public class FinalJSON
    {
        public string Idade { get; set; }
    }

}
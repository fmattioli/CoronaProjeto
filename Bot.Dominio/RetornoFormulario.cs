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
        public bool DorMuscular { get; set; }
        public bool DorArticulacoes { get; set; }
        public bool TosseSeca { get; set; }
        public bool Febre { get; set; }
        public bool Diarreia { get; set; }
        public bool RespiracaoCurtaRaza { get; set; }
        public bool SensacaoDesmaio { get; set; }
        public bool Enjoo { get; set; }
        public bool Nenhum { get; set; }
    }
    public class Respiracao
    {
        public bool FaltaAr { get; set; }
        public bool DificuldadeRespirar { get; set; }
        public bool DorPeito { get; set; }
        public bool Nenhum { get; set; }
    }
    public class DiferentesSintomas
    {
        public bool ConfusaoAlerta { get; set; }
        public bool LabiosAzulPalido { get; set; }
        public bool Nenhum { get; set; }
    }
    public class PeriodoUsuario
    {
        public string Periodo { get; set; }
    }
    public class SubstanciasUsuario
    {
        public string Substancias { get; set; }
    }

    public class FinalJSON
    {
        public string Idade { get; set; }
    }

}
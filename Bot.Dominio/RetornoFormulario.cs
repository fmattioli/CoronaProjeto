using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot.Dominio
{
    public class Escala
    {
        public string Sentimento {get; set; }
        
    }
    public class Sintomas
    {
        public bool DificuldadeRespirar { get; set; }
        public bool Febre { get; set; }
        public bool Tosse { get; set; }
        public bool DorGarganta { get; set; }
        public bool Nenhuma { get; set; }
    }
    public class PeriodoSintomas
    {
        public string Periodo { get; set; }
    }

    public class Doencas
    {
        public bool Diabetes { get; set; }
        public bool RenalCronica { get; set; }
        public bool RespiratoriaCronica { get; set; }
        public bool PressaoAlta { get; set; }
        public bool Nenhuma { get; set; }
    }

    public class Sinais
    {
        public bool Palidez { get; set; }
        public bool BocaPontaDedosRoxa { get; set; }
        public bool RespirarBaixo { get; set; }
        public bool PressaoBaixa { get; set; }
        public bool Nenhuma { get; set; }
    }


}
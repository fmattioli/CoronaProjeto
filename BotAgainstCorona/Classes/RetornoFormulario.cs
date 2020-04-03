using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotAgainstCorona.Classes
{
    public class Escala
    {
        public string Sentimento {get; set; }
        
    }
    public class Sintomas
    {
        private bool DificuldadeRespirar { get; set; }
        private bool Febre { get; set; }
        private bool Tosse { get; set; }
        private bool DorGarganta { get; set; }
        public bool Nenhuma { get; set; }
    }
    public class PeriodoSintomas
    {
        public string Periodo { get; set; }
    }

    public class Doencas
    {
        private bool Diabetes { get; set; }
        private bool RenalCronica { get; set; }
        private bool RespiratoriaCronica { get; set; }
        private bool PressaoAlta { get; set; }
        public bool Nenhuma { get; set; }
    }

    public class Sinais
    {
        private bool Palidez { get; set; }
        private bool BocaPontaDedosRoxa { get; set; }
        private bool RespirarBaixo { get; set; }
        private bool PressaoBaixa { get; set; }
        public bool Nenhuma { get; set; }
    }


}
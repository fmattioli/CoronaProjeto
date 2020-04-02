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
        public bool DificuldadeRespirar { get; set; }
        public bool Febre { get; set; }
        public bool Tosse { get; set; }
        public bool DorGarganta { get; set; }
        public bool Nenhuma { get; set; }
    }


}
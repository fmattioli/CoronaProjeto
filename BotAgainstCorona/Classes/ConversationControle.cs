using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotAgainstCorona.Classes
{
    public class ConversationControle
    {
        public string ValidarFormularioSintomas(Sintomas sintomas)
        {
            if (sintomas.Nenhuma)
            {
                return "Prosseguir para caso improvável";
            }
            else
            {
                return "Prosseguir para verificação de período de sintomas";
            }
        }

        public string ValidarDadosFormulario(string nomeForm, string retornoJSON)
        {
            switch (nomeForm)
            {
                case "form-Sintomas":
                    Sintomas sintomas = new Sintomas();
                    sintomas = JsonConvert.DeserializeObject<Sintomas>(retornoJSON);
                    return ValidarFormularioSintomas(sintomas);
                case "form-Periodo":
                    return "";

                default:
                    return "";
            }
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotAgainstCorona.Classes
{
    public class ConversationControle
    {
        public string ValidarDadosFormulario(string nomeForm, string retornoJSON)
        {
            switch (nomeForm)
            {
                case "form-Sintomas":
                    Sintomas sintomas = new Sintomas();
                    sintomas = JsonConvert.DeserializeObject<Sintomas>(retornoJSON);
                    return ValidarFormularioSintomas(sintomas);
                case "form-Sentimento":
                    Escala escala = new Escala();
                    escala = JsonConvert.DeserializeObject<Escala>(retornoJSON);
                    return ValidarFormularioEscala(escala);
              
                case "form-Periodo":
                    return "";

                default:
                    return "";
            }
        }

        public string ValidarFormularioEscala(Escala escala)
        {
            switch (escala.Sentimento)
            {
                case "MuitoMal":
                    return "Checagem de sintomas (tosse, febre, garganta)";
                case "Mal":
                    return "Checagem de sintomas (tosse, febre, garganta)";
                case "RelativamenteMal":
                    return "Checagem de sintomas (tosse, febre, garganta)";
                case "Bem":
                    return "prosseguir para verificar se o usuário deseja verificar sintomas";
                case "MuitoBem":
                    return "prosseguir para verificar se o usuário deseja verificar sintomas";
                default:
                    return "";

            }
        }

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
    }
}
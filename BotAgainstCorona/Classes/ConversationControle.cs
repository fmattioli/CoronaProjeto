using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BotAgainstCorona.Classes
{
    public class ConversationControle
    {
        public string ValidarDadosFormulario(string nomeForm, string retornoJSON)
        {
            switch (nomeForm)
            {
                case "form-Sentimento":
                    Escala escala = new Escala();
                    escala = JsonConvert.DeserializeObject<Escala>(retornoJSON);
                    return ValidarFormularioEscala(escala);

                case "form-Sintomas":
                    Sintomas sintomas = new Sintomas();
                    sintomas = JsonConvert.DeserializeObject<Sintomas>(retornoJSON);
                    return ValidarFormularioSintomas(sintomas);

                case "form-Periodo":
                    PeriodoSintomas periodo = new PeriodoSintomas();
                    periodo = JsonConvert.DeserializeObject<PeriodoSintomas>(retornoJSON);
                    return ValidarFormularioPeriodoSintomas(periodo);
                case "form-Doencas":
                    Doencas doencas = new Doencas();
                    doencas = JsonConvert.DeserializeObject<Doencas>(retornoJSON);
                    return ValidarFormularioDoencas(doencas);
                case "form-Sinais":
                    Sinais sinais = new Sinais();
                    sinais = JsonConvert.DeserializeObject<Sinais>(retornoJSON);
                    return ValidarFormularioSinais(sinais);


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
        public string ValidarFormularioPeriodoSintomas(PeriodoSintomas sintomas)
        {
            switch (sintomas.Periodo)
            {
                case "Hoje":
                    return "usuário sente sintomas há 7 dias";
                case "1Semana":
                    return "usuário com sintomas há mais de uma semana";
                case "2Semanas":
                    return "usuário com sintomas há mais de uma semana";
                case "3Semanas":
                    return "usuário com sintomas há mais de uma semana";
                default:
                    return "";

            }
        }

        public string ValidarFormularioSintomas(Sintomas sintomas)
        {
            List<int> itensFalsos = new List<int>();
            PropertyInfo[] properties = typeof(Sintomas).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(sintomas);
                if (bool.Parse(value.ToString()))
                {
                    if (sintomas.Nenhuma)
                    {
                        return "Prosseguir para caso improvável";
                    }
                    else
                    {
                        return "verificar quantidade de dias que o usuário está sentindo os sintomas";
                    }
                }
                else
                {
                    itensFalsos.Add(1);
                    if (itensFalsos.Count == 5)
                        return "formulário de sintomas preenchido de forma incorreta";
                }
            }
            return "formulário de sintomas preenchido de forma incorreta";
        }

        public string ValidarFormularioDoencas(Doencas doencas)
        {
            List<int> itensFalsos = new List<int>();
            PropertyInfo[] properties = typeof(Doencas).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(doencas);
                if (bool.Parse(value.ToString()))
                {
                    if (doencas.Nenhuma)
                    {
                        return "Prosseguir para caso improvável";
                    }
                    else
                    {
                        return "Verificar sinais do usuário";
                    }
                }
                else
                {
                    itensFalsos.Add(1);
                    if (itensFalsos.Count == 5)
                        return "formulário de sintomas preenchido de forma incorreta";
                }
            }
            return "formulário de sintomas preenchido de forma incorreta";
        }

        public string ValidarFormularioSinais(Sinais sinais)
        {
            List<int> itensFalsos = new List<int>();
            PropertyInfo[] properties = typeof(Sinais).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(sinais);
                if (bool.Parse(value.ToString()))
                {
                    if (sinais.Nenhuma)
                    {
                        return "Usuário com sintomas e sinais comuns para o corona virus";
                    }
                    else
                    {
                        return "Usuário com sintomas e sinais comuns para o corona virus";
                    }
                }
                else
                {
                    itensFalsos.Add(1);
                    if (itensFalsos.Count == 5)
                        return "formulário de sintomas preenchido de forma incorreta";
                }
            }
            return "formulário de sintomas preenchido de forma incorreta";
        }
    }
}
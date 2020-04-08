using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Bot.Dominio
{
    public class ConversationControle
    {
        Dictionary<string, string> itemValue = new Dictionary<string, string>();
        
        public  string RetornarProximaIntent(string json)
        {
            string nomeForm = string.Empty;
            json = json.ToString().Replace("{{", "{{").Replace("}}", "}");
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var retornoJSON = jss.Deserialize<dynamic>(json);
            foreach (var item in retornoJSON)
            {
                nomeForm = item.Key;
                break; //necessário passar só uma vez
            }

            return ValidarDadosFormulario(nomeForm, json);
        }

        public string ValidarDadosFormulario(string nomeForm, string retornoJSON)
        {
            string RetornoValidacao = "";
            switch (nomeForm)
            {
                case "Idade":
                    IdadeUsuario idade = new IdadeUsuario();
                    idade = JsonConvert.DeserializeObject<IdadeUsuario>(retornoJSON);
                    RetornoValidacao = ValidateAnswerOneOption(idade);
                    return RetornoValidacao == null ? "Prosseguir para armazenar o sexo do usuário" : RetornoValidacao;
                case "Sexo":
                    SexoUsuario sexo = new SexoUsuario();
                    sexo = JsonConvert.DeserializeObject<SexoUsuario>(retornoJSON);
                    RetornoValidacao = ValidateAnswerOneOption(sexo);
                    return RetornoValidacao == null ? "Prosseguir para selecionar as doencas que o usuário tem ou já teve" : RetornoValidacao;
                case "Doencas":
                    Doencas doencas = new Doencas();
                    doencas = JsonConvert.DeserializeObject<Doencas>(retornoJSON);
                    RetornoValidacao = ValidarFormularioDoencas(doencas);
                    return RetornoValidacao == null ? "Prosseguir para a verificar os sintomas que o usuário está sentindo ou já sentiu" : RetornoValidacao;
                case "Sintomas":
                    Sintomas sintomas = new Sintomas();
                    sintomas = JsonConvert.DeserializeObject<Sintomas>(retornoJSON);
                    RetornoValidacao = ValidarFormularioDoencas(sintomas);
                    return RetornoValidacao == null ? "Prosseguir para a verificar os sintomas que o usuário está sentindo ou já sentiu" : RetornoValidacao;
                case "OutrosSintomas":
                    OutrosSintomas outrosSintomas = new OutrosSintomas();
                    outrosSintomas = JsonConvert.DeserializeObject<OutrosSintomas>(retornoJSON);
                    RetornoValidacao = ValidarFormularioDoencas(outrosSintomas);
                    return RetornoValidacao == null ? "Prosseguir para checar a respiracao do usuário" : RetornoValidacao;
                case "Respiracao":
                    Respiracao respiracao = new Respiracao();
                    respiracao = JsonConvert.DeserializeObject<Respiracao>(retornoJSON);
                    RetornoValidacao = ValidarFormularioDoencas(respiracao); 
                    return RetornoValidacao == null ? "Prosseguir para checar sintomas diferenciados" : RetornoValidacao;
                case "DiferentesSintomas":
                    DiferentesSintomas diferentesSintomas = new DiferentesSintomas();
                    diferentesSintomas = JsonConvert.DeserializeObject<DiferentesSintomas>(retornoJSON);
                    RetornoValidacao = ValidarFormularioDoencas(diferentesSintomas); 
                    return RetornoValidacao == null ? "Prosseguir para o periodo de tempo dos sintomas" : RetornoValidacao;
                case "Periodo":
                    PeriodoUsuario periodoUsuario= new PeriodoUsuario();
                    periodoUsuario = JsonConvert.DeserializeObject<PeriodoUsuario>(retornoJSON);
                    RetornoValidacao = ValidateAnswerOneOption(periodoUsuario);
                    return RetornoValidacao == null ? "Prosseguir para o uso de substancias viciantes" : RetornoValidacao;
                case "Substancias":
                    SubstanciasUsuario substanciasUsuario = new SubstanciasUsuario();
                    substanciasUsuario = JsonConvert.DeserializeObject<SubstanciasUsuario>(retornoJSON);
                    RetornoValidacao = ValidateAnswerOneOption(substanciasUsuario);
                    return RetornoValidacao == null ? "Prosseguir para o resultado" : RetornoValidacao;

                //    Escala escala = new Escala();
                //    escala = JsonConvert.DeserializeObject<Escala>(retornoJSON);
                //    return ValidarResposta(escala);

                //case "form-Sintomas":
                //    Sintomas sintomas = new Sintomas();
                //    sintomas = JsonConvert.DeserializeObject<Sintomas>(retornoJSON);
                //    return ValidarFormularioSintomas(sintomas);

                //case "form-Periodo":
                //    PeriodoSintomas periodo = new PeriodoSintomas();
                //    periodo = JsonConvert.DeserializeObject<PeriodoSintomas>(retornoJSON);
                //    return ValidarFormularioPeriodoSintomas(periodo);

                //case "form-Sinais":
                //    Sinais sinais = new Sinais();
                //    sinais = JsonConvert.DeserializeObject<Sinais>(retornoJSON);
                //    return ValidarFormularioSinais(sinais);


                default:
                    return null;
            }
        }

        public string ValidateAnswerOneOption(object retorno)
        {
            var objetoTipo = retorno.GetType();
            PropertyInfo[] properties = objetoTipo.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(retorno).ToString();
                if(value.ToString() == "")
                {
                    return "Erro";
                }
                else
                { 
                    return null;
                }
            }

            return "Erro";
        }

        private string AppendJson(object objeto)
        {
            var objetoTipo = objeto.GetType();
            PropertyInfo[] properties = objetoTipo.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(objeto).ToString();
                itemValue.Add(name, value);
                if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                {
                    return "Ocorreu um erro na validação da sua resposta! Vamos tentar novamente";
                }
            }
            return null;
        }

        
        public string ValidarFormularioDoencas(object doencas)
        {
            var tipo = doencas.GetType();
            List<int> itensFalsos = new List<int>();
            PropertyInfo[] properties = tipo.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(doencas);
                if (bool.Parse(value.ToString()))
                {
                    if (property.Name == "Nenhuma" && bool.Parse(value.ToString()))
                    {
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    itensFalsos.Add(1);
                    if (itensFalsos.Count == properties.Count())
                    {
                        switch (tipo.Name)
                        {
                            case "Doencas":
                                return "formulário de doencas que o usuário já teve está preenchido de forma incorreta";
                            case "Sintomas":
                                return "formulário de sintomas preenchido de forma incorreta";
                            case "OutrosSintomas":
                                return "formulário de sintomas preenchido de forma incorreta";
                            case "Respiracao":
                                return "formulário de sintomas preenchido de forma incorreta";
                            case "DiferentesSintomas":
                                return "formulário de sintomas preenchido de forma incorreta";
                            case "Periodo":
                                return "formulário de sintomas preenchido de forma incorreta";
                            case "Substancias":
                                return "formulário de sintomas preenchido de forma incorreta";
                            default:
                                break;
                        }
                       
                    }
                }
            }
            return "formulário de sintomas preenchido de forma incorreta";
        }
        
        
    }
}
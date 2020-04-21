using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;

namespace Bot.Dados
{
    [Serializable]
    public class Dados
    {
        private void Conexao(string query, bool fechar = false)
        {
            try
            {
                String ConnectionString = "Data Source = botagainstcorona.database.windows.net; User ID = botagainstcorona; Password = takakicoach@2020";
                SqlConnection conn = new SqlConnection();
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = query;
                command.ExecuteNonQuery();
                if (fechar)
                    conn.Close();
            }
            catch(Exception erro)
            {
                throw new Exception("Ocorreu o seguinte erro: " + erro.Message.ToLower());
            }
        }
        private void ExecutarCommand(string query)
        {
            try
            {
                using (var connection1 = new SqlConnection("Server = botagainstcorona.database.windows.net; Database = BotAgainstCorona; User Id = botagainstcorona; Password = takakicoach@2020"))//@"Data Source = botagainstcorona.database.windows.net; User ID = botagainstcorona; Password = takakicoach@2020"))
                using (var cmd = new SqlDataAdapter())
                using (var insertCommand = new SqlCommand(query))
                {
                    insertCommand.Connection = connection1;
                    cmd.InsertCommand = insertCommand;
                    connection1.Open();
                    int rowsAffect = cmd.InsertCommand.ExecuteNonQuery();
                    if(rowsAffect < 1)
                    {
                        throw new Exception("Nenhum dado foi inserido");
                    }
                }
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
            
        }

        public void InserirDadosConversa(string nomeCard, string json)
        { 
            var jsonString = JsonConvert.SerializeObject(json);
            jsonString = "{" + jsonString + "}";
            int numConfigConversa = RetornarIDConfigConversa(nomeCard);
            numConfigConversa = numConfigConversa == 0 ? throw new Exception("A configuração não foi encontrada") : numConfigConversa;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into ConversaBot");
            sql.AppendLine("(");
            sql.AppendLine("DataConversa,");
            sql.AppendLine("JsonConversa,");
            sql.AppendLine("FK_ConfigID");
            sql.AppendLine(")");
            sql.AppendLine("Values");
            sql.AppendLine("(");
            sql.AppendLine($"'{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}',");
            sql.AppendLine($"'{jsonString}',");
            sql.AppendLine($"{numConfigConversa }");
            sql.AppendLine($")");
            ExecutarCommand(sql.ToString());
         
        }

        private int RetornarIDConfigConversa(string nomeCard)
        {
            switch (nomeCard)
            {
                case "Idade":
                    return 1;
                case "Sexo":
                    return 2;
                case "Doencas":
                    return 3;  
                default:
                    return 0;
            }
        }
    
    }
}

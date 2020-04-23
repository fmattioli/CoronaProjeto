using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System.Net;
using System.Text;
using System;
using System.Threading;
using Google.Apis.Util.Store;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Linq;

namespace Bot.Data
{
    [Serializable]
    public class ApiGoogleSheets
    {

        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static string Sheet = "DadosPessoais";
        static SheetsService service;
        static string spreadsheetId;

        public ApiGoogleSheets()
        {
            try
            {
                GoogleCredential credential;

                credential = GoogleCredential.FromJson(CallURL("https://fmattiolidossantos2020.000webhostapp.com/credentials.json"))
                     .CreateScoped(Scopes);

                // Create Google Sheets API service.
                service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                spreadsheetId = "10ZdIxL3kaIRzhO4b_SjPhRA-WpIkXwJG_aXguYbPoQs";
                String range = "DadosPessoais!A1:B2";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(spreadsheetId, range);
                ValueRange response = request.Execute();
                var values = response.Values;
                //if (values != null && values.Count > 0)
                //{
                //    foreach (var row in values)
                //    {
                //        // Print columns A and E, which correspond to indices 0 and 4.
                //        Console.WriteLine("{0}, {1}", row[0], row[1]);
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("No data found.");
                //}
            }
            catch (Exception erro)
            {

            }
        }

        public void CreateEntry(string DataConversa, string JSONConversa, Planilhas nome, string PeriodoSintomas = "")
        {
            String bracktsIni = "{";
            String bracktsFim = "}";
            JSONConversa = bracktsIni + "\n" + JSONConversa + bracktsFim;
            var range = $"{GetEnumDescription(nome)}!A:B";
            var valueRange = new ValueRange();
            var objectList = new List<object>();
            if (nome != Planilhas.DadosSintomas)
            {
                objectList = new List<object>()
                {
                    DataConversa,
                    JSONConversa
                };
            }
            else
            {
                objectList = new List<object>()
                {
                    DataConversa,
                    JSONConversa,
                    PeriodoSintomas
                };
            }

            valueRange.Values = new List<IList<object>> { objectList };
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = appendRequest.Execute();
        }



        public string CallURL(String nome)
        {
            var URL = nome;
            WebClient Client = new WebClient();
            Client.Encoding = Encoding.UTF8;
            var json = Client.DownloadString(URL);
            return json;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

    }
    public enum Planilhas
    {
        [Description("DadosPessoais")]
        DadosPessoais = 1,
        [Description("DadosDoencas")]
        DadosDoencas = 2,
        [Description("DadosSintomas")]
        DadosSintomas = 3
    }

}

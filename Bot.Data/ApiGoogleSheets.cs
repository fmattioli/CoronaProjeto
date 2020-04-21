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

namespace Bot.Data
{
    [Serializable]
    public class ApiGoogleSheets
    {

        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static string Sheet = "Dados";
        static SheetsService service;
        static string spreadsheetId;

        public ApiGoogleSheets()
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
            String range = "Dados!A1:B2";
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

        public void CreateEntry(string DataConversa, string JSONConversa)
        {
            var range = $"{Sheet}!A:B";
            var valueRange = new ValueRange();
            var objectList = new List<object>()
            {
                DataConversa,
                JSONConversa
            };
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
    }
}

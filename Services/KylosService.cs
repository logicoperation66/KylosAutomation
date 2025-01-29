using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;
using HtmlAgilityPack;

namespace KylosNotify.Services
{
    public class KylosService
    {
        private readonly HtmlAgilityPack.HtmlWeb web;
        private readonly string login;
        private readonly string password;
        private readonly HttpClient httpClient;

        public KylosService()
        {
            Env.Load("C:\\Users\\adamw\\source\\repos\\KylosNotify\\KylosNotify\\.env");
            login = Environment.GetEnvironmentVariable("LOGIN");
            password = Environment.GetEnvironmentVariable("PASSWORD");
            web = new HtmlAgilityPack.HtmlWeb();
           httpClient = new HttpClient();
        }

        public async Task<bool> LoginAsync()
        {
            var loginUrl = "https://sp5.kylos.pl";
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("login", login),
                new KeyValuePair<string, string>("haslo", password)
            });

            var response = await httpClient.PostAsync(loginUrl, loginData);
            Console.WriteLine("WTF");
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<string> GetNewClassNotice()
        {
            var requestUrl = "https://sp5.kylos.pl/ajax_lista_wydarzen.php?idkl=123&&wybor=0&&max_pozycji=30&&wszystkie=0&&twoje=0&&wybrana_grupa=wszystkie&&wybrany_prz=0";
            var response = await httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public void ParseHtml(string html)
        {
            var doc = html;
            HtmlDocument htmlDock = new HtmlDocument();
            htmlDock.LoadHtml(doc);

            string dateXpath = "//td[@class='norma' and contains(@style, 'font-size:13px')]";
            string messageXpath = "//td[@class='norma' and contains(@style, 'font-size:15px')]//p";
            
            var dateNode = htmlDock.DocumentNode.SelectNodes(dateXpath);
            var messageNode = htmlDock.DocumentNode.SelectNodes(messageXpath);

            if (dateNode != null && messageNode != null && dateNode.Count == messageNode.Count)
            {
                for (int i = 0; i < messageNode.Count; i++)
                {
                    string dateText = dateNode[i].InnerText.Trim();
                    
                    string messageText = messageNode[i].InnerText.Trim();

                    
                    Console.WriteLine($"Data: {dateText}");
                    Console.WriteLine($"Treść: {messageText}");
                    Console.WriteLine(new string('-', 200));
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono danych");
            }

        }
    }
}

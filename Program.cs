using HtmlAgilityPack;
using DotNetEnv;
using System;
using KylosNotify.Services;
using System.Text;

namespace KylosNotify
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            KylosService service = new KylosService();
            if (await service.LoginAsync())
            {
                string X = await service.GetNewClassNotice();
                Console.WriteLine("Logged in");
                Console.WriteLine(X);
            }    
        }
    }
}

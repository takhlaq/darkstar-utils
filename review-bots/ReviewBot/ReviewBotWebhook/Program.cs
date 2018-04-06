using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ReviewBotWebhook
{
    public class Program
    {
        public static string User, Pass, Token;
        static void ParseArguments(string[] args)
        {
            for (int i = 0; i + 1 < args.Length; i += 2)
            {
                var arg = args[i];
                var val = args[i + 1];

                if (arg == "-User")
                    User = val;
                else if (arg == "-Pass")
                    Pass = val;
                else if (arg == "-Token")
                    Token = val;
            }
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

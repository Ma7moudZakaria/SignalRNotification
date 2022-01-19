using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace WebApplication4
{
    class Program
    {
        private static bool isConnected = false;
        private static HubConnection connection = null;

        static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //Console.WriteLine("SignalR Client Application Started");


            //Connect without Authentication
            SimpleConnection();

            //#region ' Listeners '
            

            //connection.On<string>("AddArtifact", (message) =>
            //{
            //    var newMessage = $"{message}";
            //    Console.WriteLine(newMessage);
            //});


            //connection.On<string>("connections", (message) =>
            //{
            //    var newMessage = $"{message}";
            //    Console.WriteLine(newMessage);
            //});

            //connection.On<string>("UpdateExecution", (message) =>
            //{
            //    var newMessage = $"{message}";
            //    Console.WriteLine(newMessage);
            //});


            //connection.On<string>("onlineUsers", (message) =>
            //{
            //    var newMessage = $"{message}";
            //    Console.WriteLine(newMessage);
            //});
            //#endregion


            try
            {
                await TryConnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            await connection.InvokeAsync("SendMessage", "thomas", "thomas2");
            var x = "";
            connection.On<string>("SendMessage", (message) =>
            {
                x = message;
                var newMessage = $"{message}";
                //Console.WriteLine(newMessage);
            });

            connection.Closed += Connection_Closed;

            //Console.WriteLine("Press any key to exit...");
            Console.Read();

            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().
                    UseKestrel(o => { o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10); });
                });

        private async static Task Connection_Closed(Exception arg)
        {
            Console.WriteLine("Client Disconnected...");
            isConnected = false;
            await TryConnectAsync();
        }

        public async static Task TryConnectAsync()
        {
            while (!isConnected)
            {
                await connection.StartAsync().ContinueWith((task) =>
                {
                    isConnected = true;
                    if (task.IsCompleted)
                        Console.WriteLine("Client Connected...");
                    else
                        Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                });
            }
        }


        private static void SimpleConnection()
        {
            connection = new HubConnectionBuilder()
              .WithUrl("https://localhost:44346/NotificationHub").Build();
        }

    }
}

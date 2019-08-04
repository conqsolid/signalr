using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;


namespace SignalRConsole
{
    class Program
    {
        static HubConnection connection;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            connection = new HubConnectionBuilder().WithUrl("http://localhost:5002/chathub").Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(1000);
                Console.WriteLine($"Connection closed. Error: {error.ToString()}");
                connect();
            };

            connection.On<string, string, string>("broadcastMessage", (user, message, id) =>
            {
                Console.WriteLine($"GetMessage:{ user}:{message}:{id}");
            });

            while (true)
            {
                var key = Console.Read();
                if (key == 'c')
                {
                    connect();
                }
                else if (key == 'd')
                {
                    disconnect();
                }

            }
        }

        private static HubConnectionState disconnect()
        {
            try
            {
                connection.StopAsync().ContinueWith((t) =>
                {
                    Console.WriteLine($"Disconnect : Connection State: {connection.State}");

                }
                );
                return connection.State;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Disconnect exception occured: {ex.ToString()}");
                return HubConnectionState.Disconnected;
            }
        }

        private static HubConnectionState connect()
        {
            try
            {
                connection.StartAsync().ContinueWith((t) =>
                {
                    Console.WriteLine($"Connect : Connection State: {connection.State}");
                }
                );
                return connection.State;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connect exception occured: {ex.ToString()}");
                return HubConnectionState.Disconnected;
            }
        }
    }
}

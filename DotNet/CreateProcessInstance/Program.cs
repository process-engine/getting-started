namespace LagerTeilautomatisch
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using ProcessEngineClient;

    internal class Program
    {
        private static void Main(string[] args)
        {
            StartNewProcessInstance().GetAwaiter().GetResult();
        }

        private static async Task StartNewProcessInstance() 
        {
            var client = new ProcessEngineClient("http://localhost:56000");

            var request = new ProcessStartRequest<StartPayload>();
            request.Payload.ShoppingCardAmount = 1000;

            Console.WriteLine("Prozess 'Benutzeraktivierung' mit Start-Event 'StartAktivierung' gestartet.");

            var result = await client.StartProcessInstance<StartPayload, object>(
                "Benutzeraktivierung", 
                "StartAktivierung",
                request, 
                "EndeAktivierung");
   
            Console.WriteLine($"Prozess beendet (CorrelationId: '{result.CorrelationId}').");
            Console.Write("Daten: ");
            Console.WriteLine(result.Payload);
        }
    }
}

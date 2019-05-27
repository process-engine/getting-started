namespace LagerTeilautomatisch
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using ProcessEngineClient;

    class Program
    {
        static void Main(string[] args)
        {
            StartProcessNew().GetAwaiter().GetResult();
        }

        static async Task StartProcessNew() 
        {
            //ProcessEngineClient client = new ProcessEngineClient("http://localhost:8000", Identity.DefaultIdentity);
            ProcessEngineClient client = new ProcessEngineClient("http://localhost:8000");

            ProcessStartRequest<StartPayload> request = new ProcessStartRequest<StartPayload>();
            request.Payload.ShoppingCardAmount = 1000;

            Console.WriteLine($"Prozess gestartet '{"Benutzeraktivierung"}' mit Start-Event '{"StartAktivierung"}'.");

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

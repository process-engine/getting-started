namespace LagerTeilautomatisch
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using EssentialProjects.IAM.Contracts;

    using global::ProcessEngine.ConsumerAPI.Client;
    using global::ProcessEngine.ConsumerAPI.Contracts;
    using global::ProcessEngine.ConsumerAPI.Contracts.DataModel;

    using ProcessEngine;
    using Identity = ProcessEngine.Identity;

    class Program
    {
        const string PROCESS_MODEL_ID= "Benutzeraktivierung";

        const string START_EVENT_ID = "StartAktivierung";

        const string END_EVENT_ID = "EndeAktivierung";

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

            Console.WriteLine($"Prozess gestartet '{PROCESS_MODEL_ID}' mit Start-Event '{START_EVENT_ID}'.");

            ProcessStartResponsePayload result = await client.StartProcessInstance<StartPayload>(
                PROCESS_MODEL_ID, START_EVENT_ID,
                request, 
                StartCallbackType.CallbackOnEndEventReached,
                END_EVENT_ID);
   
            Console.WriteLine($"Prozess beendet (CorrelationId: '{result.CorrelationId}').");
            Console.Write("Daten: ");
            Console.WriteLine(result.TokenPayload);
        }
    }
}

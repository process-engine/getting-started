namespace LagerTeilautomatisch
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using EssentialProjects.IAM.Contracts;

    using ProcessEngine.ConsumerAPI.Client;
    using ProcessEngine.ConsumerAPI.Contracts;
    using ProcessEngine.ConsumerAPI.Contracts.DataModel;

    class Program
    {
        const string PROCESS_MODEL_ID= "Benutzeraktivierung";


        const string START_EVENT_ID = "StartAktivierung";

        const string END_EVENT_ID = "EndeAktivierung";

        static void Main(string[] args)
        {
            StartProcess().GetAwaiter().GetResult();
        }

        static async Task StartProcess() {

            ConsumerApiClientService client = Program.CreateConsumerClient("http://localhost:8000");

            ProcessStartRequestPayload<StartPayload> payload = Program.CreatePayload(1000);

            IIdentity identity = CreateIdentity();

            Console.WriteLine($"Prozess gestartet '{PROCESS_MODEL_ID}' mit Start-Event '{START_EVENT_ID}'.");

            ProcessStartResponsePayload result = await client.StartProcessInstance<StartPayload>(
                identity, 
                PROCESS_MODEL_ID, 
                START_EVENT_ID, 
                payload, 
                StartCallbackType.CallbackOnEndEventReached,
                END_EVENT_ID);

            Console.WriteLine($"Prozess beendet (CorrelationId: '{result.CorrelationId}').");
            Console.Write("Daten: ");
            Console.WriteLine(result.TokenPayload);
        }

        static internal ConsumerApiClientService CreateConsumerClient(string url) 
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);

            ConsumerApiClientService client = new ConsumerApiClientService(httpClient);

            return client;
        }

        static internal ProcessStartRequestPayload<StartPayload> CreatePayload(int inputAmount) 
        {
            StartPayload startPayload = new StartPayload();
            startPayload.ShoppingCardAmount = inputAmount;

            var processStartPayload = new ProcessStartRequestPayload<StartPayload>();
            processStartPayload.InputValues = startPayload;

            return processStartPayload;
        }

        static internal IIdentity CreateIdentity() 
        {
            return new Identity() { Token = Convert.ToBase64String(Encoding.UTF8.GetBytes("dummy_token")) };
        }
    }
}

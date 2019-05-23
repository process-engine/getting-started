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
        const string PROCESS_MODEL_ID= "Lager-Manuell";


        const string START_EVENT_ID = "VersandauftragErhalten";

        const string END_EVENT_ID = "VersandauftragVersendet";

        static void Main(string[] args)
        {
            StartProcess().GetAwaiter().GetResult();
        }

        static async Task StartProcess() {

            var client = CreateConsumerApiClient("http://localhost:8000");

            var payload = CreateStartRequestPayload("Dies ist die Eingabe für den Prozess aus DotNet.");

            var identity = CreateIdentity();

            Console.WriteLine($"Prozess '{PROCESS_MODEL_ID}' mit Start-Event '{START_EVENT_ID}' gestartet.");

            var result = await client.StartProcessInstance<CustomStartPayload>(
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

        static internal ConsumerApiClientService CreateConsumerApiClient(string url)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);

            var client = new ConsumerApiClientService(httpClient);

            return client;
        }

        static internal ProcessStartRequestPayload<CustomStartPayload> CreateStartRequestPayload(string inputText)
        {
            var startPayload = new CustomStartPayload();
            startPayload.InputProperty = inputText;

            var processStartPayload = new ProcessStartRequestPayload<CustomStartPayload>();
            processStartPayload.InputValues = startPayload;

            return processStartPayload;
        }

        static internal IIdentity CreateIdentity()
        {
            return new Identity() { Token = Convert.ToBase64String(Encoding.UTF8.GetBytes("dummy_token")) };
        }
    }
}

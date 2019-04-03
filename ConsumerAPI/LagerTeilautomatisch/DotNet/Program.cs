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
        static Uri ProcessEngineBaseUri = new Uri("http://localhost:8000");

        const string PROCESS_MODEL_ID= "Lager-Teilautomatisch";
        const string START_EVENT_KEY = "VersandauftragErhalten";

        static void Main(string[] args)
        {
            StartProcess().GetAwaiter().GetResult();
        }

        static async Task StartProcess() {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = Program.ProcessEngineBaseUri;

            IIdentity identity = CreateIdentity();

            ConsumerApiClientService client = new ConsumerApiClientService(httpClient);

            StartPayload startPayload = new StartPayload();

            var processStartPayload = new ProcessStartRequestPayload<StartPayload>();
            processStartPayload.InputValues = startPayload;

            var result = await client.StartProcessInstance<StartPayload>(identity, PROCESS_MODEL_ID, START_EVENT_KEY, processStartPayload);
        }

        static internal IIdentity CreateIdentity() 
        {
            return new Identity() { Token = Convert.ToBase64String(Encoding.UTF8.GetBytes("dummy_token")) };
        }

    }
}

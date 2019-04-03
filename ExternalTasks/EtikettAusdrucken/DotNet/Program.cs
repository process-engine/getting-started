namespace EtikettAusdrucken
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    
    using Newtonsoft.Json;
    
    using ProcessEngine.ExternalTaskAPI.Client;
    using ProcessEngine.ExternalTaskAPI.Contracts;

    class Program
    {
        static void Main(string[] args)
        {
            RunWorker().GetAwaiter().GetResult();
        }

        private static async Task RunWorker()
        {
            IIdentity identity = new TestIdentity();
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:8000");

            IExternalTaskAPI externalTaskApi = new ExternalTaskApiClientService(client);
            ExternalTaskWorker externalTaskWorker = new ExternalTaskWorker(externalTaskApi);

            await externalTaskWorker.WaitForHandle<TestPayload>(identity, "TestTopic", 10, 10000, async (externalTask) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(externalTask));

                await Task.Delay(40000);

                return new ExternalTaskFinished<TestResult>(externalTask.Id, new TestResult());
            });
        }    
    }
}

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
        const string TOPIC = "AktivierungsemailSenden";

        const int MAX_TASKS = 10;

        const int POLLING_TIMEOUT = 1000;
        const int WAIT_TIMEOUT = 10000;

        static void Main(string[] args)
        {
            RunWorker().GetAwaiter().GetResult();
        }

        private static async Task RunWorker()
        {
            ExternalTaskWorker externalTaskWorker = Program.CreateExternalTaskWorker("http://localhost:8000");

            IIdentity identity = new TestIdentity();

            Console.WriteLine($"Warten auf Aufgaben für das Topic '{TOPIC}'.");
            
            await externalTaskWorker.WaitForHandle<TestPayload>(identity, TOPIC, MAX_TASKS, POLLING_TIMEOUT, async (externalTask) =>
            {
                Console.WriteLine("");
                Console.Write("Daten: ");
                Console.Write(JsonConvert.SerializeObject(externalTask));
                Console.WriteLine("");
                Console.WriteLine("");

                var result = await Program.DoSomeLongWork();

                var externalTaskFinished = new ExternalTaskFinished<TestResult>(externalTask.Id, result);

                return externalTaskFinished;
            });
        } 

        private async static Task<TestResult> DoSomeLongWork() 
        {
            var result = new TestResult();
            result.ShoppingCardAmount = 1000;

            Console.WriteLine($"Warte für {WAIT_TIMEOUT} Millisekunden.");
            await Task.Delay(WAIT_TIMEOUT);

            Console.WriteLine("Bearbeitung fertig!");

            return result;
        }

        private static ExternalTaskWorker CreateExternalTaskWorker(string url) 
        {
            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(url);

            IExternalTaskAPI externalTaskApi = new ExternalTaskApiClientService(httpClient);

            ExternalTaskWorker externalTaskWorker = new ExternalTaskWorker(externalTaskApi);

            return externalTaskWorker;   
        }
    }
}

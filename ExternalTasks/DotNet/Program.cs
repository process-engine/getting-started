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
        const string TOPIC = "Etikett-ausdrucken";

        const int MAX_TASKS = 10;

        const int POLLING_TIMEOUT = 1000;
        const int WAIT_TIMEOUT = 10000;

        static void Main(string[] args)
        {
            RunWorker().GetAwaiter().GetResult();
        }

        private static async Task RunWorker()
        {
            var externalTaskWorker = CreateExternalTaskWorker("http://localhost:8000");

            var sampleIdentity = new TestIdentity();

            Console.WriteLine($"Warten auf Aufgaben für das Topic '{TOPIC}'.");

            await externalTaskWorker.WaitForHandle<TestPayload>(sampleIdentity, TOPIC, MAX_TASKS, POLLING_TIMEOUT, async (externalTask) =>
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
            result.TestProperty = "Dies ist das Ergebnis vom DotNet-External-Task.";

            Console.WriteLine($"Warte für {WAIT_TIMEOUT} Millisekunden.");
            await Task.Delay(WAIT_TIMEOUT);

            Console.WriteLine("ExternalTask erfolgreich bearbeitet!");

            return result;
        }

        private static ExternalTaskWorker CreateExternalTaskWorker(string url)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(url);

            var externalTaskApi = new ExternalTaskApiClientService(httpClient);

            var externalTaskWorker = new ExternalTaskWorker(externalTaskApi);

            return externalTaskWorker;
        }
    }
}

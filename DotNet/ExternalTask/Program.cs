namespace EtikettAusdrucken
{
    using System;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using ProcessEngine.ConsumerAPI.Contracts.DataModel;

    using ProcessEngineClient;

    internal class Program
    {
        private static void Main(string[] args)
        {
            RunSampleExternalTaskWorker().GetAwaiter().GetResult();
        }

        private static async Task RunSampleExternalTaskWorker()
        {
            var client = new ProcessEngineClient("http://localhost:56000");

            Console.WriteLine("Warten auf Aufgaben für das Topic 'AktivierungsemailSenden'.");

            await client.SubscribeToExternalTasksWithTopic<TestPayload, Task<TestResult>>("AktivierungsemailSenden", async (externalTask) => {
                Console.WriteLine("");
                Console.Write("Daten: ");
                Console.Write(JsonConvert.SerializeObject(externalTask));
                Console.WriteLine("");
                Console.WriteLine("");

                var result = await DoSomeLongWork(externalTask.Payload);

                var externalTaskFinished = new ExternalTaskSuccessResult<TestResult>(externalTask.Id, result);

                return externalTaskFinished;
            });
        }

        private async static Task<TestResult> DoSomeLongWork<TPayload>(TPayload payload)
        {
            if (!(payload is TestPayload testPayload))
            {
                return null;
            }

            var result = new TestResult();
            result.ShoppingCardAmount = testPayload.ShoppingCardAmount;

            Console.WriteLine("Warte für 10000 Millisekunden.");
            await Task.Delay(10000);

            Console.WriteLine("Bearbeitung fertig!");

            return result;
        }
    }
}

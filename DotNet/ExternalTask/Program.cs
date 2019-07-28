namespace EtikettAusdrucken
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    
    using Newtonsoft.Json;
    
    using ProcessEngine.ExternalTaskAPI.Client;
    using ProcessEngine.ExternalTaskAPI.Contracts;

    using ProcessEngineClient;

    internal class Program
    {
        private static void Main(string[] args)
        {
            RunSampleExternalTaskWorker().GetAwaiter().GetResult();
        }

        private static async Task RunSampleExternalTaskWorker()
        {
            var client = new ProcessEngineClient("http://localhost:8000");

            Console.WriteLine("Warten auf Aufgaben für das Topic 'AktivierungsemailSenden'.");

            await client.SubscribeToExternalTasksWithTopic<TestPayload>("PapermillTopic", async (externalTask) => {
                Console.WriteLine("");
                Console.Write("Daten: ");
                Console.Write(JsonConvert.SerializeObject(externalTask));
                Console.WriteLine("");
                Console.WriteLine("");

                //var result = await DoSomeLongWork(externalTask.Payload);

                //var externalTaskFinished = new ExternalTaskFinished<TestResult>(externalTask.Id, result);
                var externalTaskBpmnError = new ExternalTaskBpmnError(externalTask.Id, "error_message");

                return externalTaskBpmnError;
            });
        }

        private async static Task<TestResult> DoSomeLongWork(TestPayload payload) 
        {
            var result = new TestResult();
            result.ShoppingCardAmount = payload.ShoppingCardAmount;

            Console.WriteLine("Warte für 10000 Millisekunden.");
            await Task.Delay(10000);

            Console.WriteLine("Bearbeitung fertig!");

            return result;
        }
    }
}

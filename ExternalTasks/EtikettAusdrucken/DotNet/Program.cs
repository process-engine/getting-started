﻿namespace EtikettAusdrucken
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

        static Uri ProcessEngineBaseUri = new Uri("http://localhost:8000");

        const string TOPIC = "Etikett-ausdrucken";

        const int MAX_TASKS = 10;

        const int POLLING_TIMEOUT = 1000;
        const int WAIT_TIMEOUT = 40000;

        static void Main(string[] args)
        {
            RunWorker().GetAwaiter().GetResult();
        }

        private static async Task RunWorker()
        {
            IIdentity identity = new TestIdentity();
            HttpClient client = new HttpClient();

            client.BaseAddress = Program.ProcessEngineBaseUri;

            IExternalTaskAPI externalTaskApi = new ExternalTaskApiClientService(client);
            ExternalTaskWorker externalTaskWorker = new ExternalTaskWorker(externalTaskApi);

            Console.WriteLine("Waiting for process-engine-tasks.");

            await externalTaskWorker.WaitForHandle<TestPayload>(identity, TOPIC, MAX_TASKS, POLLING_TIMEOUT, async (externalTask) =>
            {
                Console.Write("PAYLOAD: ");
                Console.Write(JsonConvert.SerializeObject(externalTask));
                Console.WriteLine("");


                Console.WriteLine($"Waitung for {WAIT_TIMEOUT} seconds.");
                await Task.Delay(WAIT_TIMEOUT);

                Console.WriteLine("Work done!");
                return new ExternalTaskFinished<TestResult>(externalTask.Id, new TestResult());
            });
        }    
    }
}

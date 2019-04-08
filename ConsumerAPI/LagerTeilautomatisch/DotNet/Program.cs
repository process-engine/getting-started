﻿namespace LagerTeilautomatisch
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
        const string PROCESS_MODEL_ID= "Lager-Teilautomatisch";
        const string START_EVENT_ID = "VersandauftragErhalten";

        const string END_EVENT_ID = "VersandauftragVersendet";

        static void Main(string[] args)
        {
            StartProcess().GetAwaiter().GetResult();
        }

        static async Task StartProcess() {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8000");

            ConsumerApiClientService client = new ConsumerApiClientService(httpClient);

            var processStartRequestPayload = new ProcessStartRequestPayload<StartPayload>();

            var startPayload = new StartPayload();
            processStartRequestPayload.InputValues = startPayload;

            IIdentity identity = CreateIdentity();

            Console.WriteLine($"Prozess gestartet '{PROCESS_MODEL_ID}' beim Start-Event '{START_EVENT_ID}'.");

            ProcessStartResponsePayload result = await client.StartProcessInstance<StartPayload>(
                identity, 
                PROCESS_MODEL_ID, 
                START_EVENT_ID, 
                processStartRequestPayload, 
                StartCallbackType.CallbackOnEndEventReached,
                END_EVENT_ID);

            Console.WriteLine($"Prozess beendet (CorrelationId: '{result.CorrelationId}').");
            Console.Write("Daten: ");
            Console.WriteLine(result.TokenPayload);
        }

        static internal IIdentity CreateIdentity() 
        {
            return new Identity() { Token = Convert.ToBase64String(Encoding.UTF8.GetBytes("dummy_token")) };
        }

    }
}

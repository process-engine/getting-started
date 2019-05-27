namespace LagerTeilautomatisch.ProcessEngine
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using global::ProcessEngine.ConsumerAPI.Client;
    using global::ProcessEngine.ConsumerAPI.Contracts;
    using global::ProcessEngine.ConsumerAPI.Contracts.DataModel;

    internal class ProcessEngineClient 
    {
        private HttpClient HttpClient { get; }

        private Identity Identity {get; set;}

        private ConsumerApiClientService ConsumerApiClient { get; }

        public ProcessEngineClient(string url) 
            : this(url, Identity.DefaultIdentity)
        {   
        }

        public ProcessEngineClient(string url, Identity identity) 
        {
            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(url);
            this.Identity = identity;

            this.ConsumerApiClient = new ConsumerApiClientService(this.HttpClient);
        }

        public async Task<ProcessStartResponsePayload> StartProcessInstance<TPayload>(
            string processModelId, 
            string startEventId, 
            ProcessStartRequest<TPayload> processStartRequestPayload, 
            StartCallbackType callbackType = StartCallbackType.CallbackOnProcessInstanceCreated,
            string endEventId = "")
            where TPayload : new() 
        {
            ProcessStartRequestPayload<TPayload> payload = new ProcessStartRequestPayload<TPayload>();

            payload.InputValues = processStartRequestPayload.Payload;

            return await this.ConsumerApiClient.StartProcessInstance<TPayload>(
                this.Identity.InternalIdentity, 
                processModelId,
                startEventId,
                payload,
                callbackType,
                endEventId);

        }
    }
}
namespace ProcessEngineClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using global::ProcessEngine.ConsumerAPI.Client;
    using global::ProcessEngine.ConsumerAPI.Contracts;
    using global::ProcessEngine.ConsumerAPI.Contracts.DataModel;

    public class ProcessEngineClient 
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

        public async Task<ProcessStartResponse<TResponsePayload>> StartProcessInstance<TRequestPayload, TResponsePayload>(
            string processModelId, 
            string startEventId, 
            ProcessStartRequest<TRequestPayload> request, 
            string endEventId = "")
            where TRequestPayload : new()
            where TResponsePayload: new() 
        {
            var callbackType = StartCallbackType.CallbackOnEndEventReached;

            var payload = new ProcessStartRequestPayload<TRequestPayload>();

            payload.CallerId = request.ParentProcessInstanceId;
            payload.CorrelationId = request.CorrelationId;
            payload.InputValues = request.Payload;

            var responsePayload = await this.ConsumerApiClient.StartProcessInstance<TRequestPayload>(
                this.Identity.InternalIdentity, 
                processModelId,
                startEventId,
                payload,
                callbackType,
                endEventId);

            var response = new ProcessStartResponse<TResponsePayload>();
            response.ProcessInstanceId = responsePayload.ProcessInstanceId;
            response.CorrelationId = responsePayload.CorrelationId;
            response.EndEventId = responsePayload.EndEventId;

            try 
            {
                response.Payload = (TResponsePayload)responsePayload.TokenPayload;
            } 
            catch (Exception e) 
            {
                Console.WriteLine(e);
            }

            return response;
        }
    }
}
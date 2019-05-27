namespace ProcessEngineClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using ProcessEngine.ConsumerAPI.Client;
    using ProcessEngine.ConsumerAPI.Contracts;
    using ProcessEngine.ConsumerAPI.Contracts.DataModel;

    using ProcessEngine.ExternalTaskAPI.Client;
    using ProcessEngine.ExternalTaskAPI.Contracts;


    public class ProcessEngineClient 
    {
        private HttpClient HttpClient { get; }

        private Identity Identity {get; set;}

        private ConsumerApiClientService ConsumerApiClient { get; }

        private IExternalTaskAPI ExternalTaskApi { get; }


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

            this.ExternalTaskApi = new ExternalTaskApiClientService(this.HttpClient);
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

        public async Task WaitForHandle<TPayload>(string topic, HandleExternalTaskAction<TPayload> handleAction) 
            where TPayload : new()
        {
            int maxTasks = 10;
            int longpollingTimeout = 1000;

            ExternalTaskWorker externalTaskWorker = new ExternalTaskWorker(this.ExternalTaskApi);

            await externalTaskWorker.WaitForHandle(this.Identity.ExternalTaskIdentity, topic, maxTasks, longpollingTimeout, handleAction);
        }

    }
}
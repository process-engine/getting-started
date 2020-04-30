namespace ProcessEngineClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using ProcessEngine.ConsumerAPI.Client;
    using ProcessEngine.ConsumerAPI.Contracts;
    using ProcessEngine.ConsumerAPI.Contracts.APIs;
    using ProcessEngine.ConsumerAPI.Contracts.DataModel;

    using ProcessEngine.ExternalTaskAPI.Client;

    public class ProcessEngineClient
    {
        private HttpClient HttpClient { get; }

        private Identity Identity {get; set;}

        private ConsumerApiClientService ConsumerApiClient { get; }

        private IExternalTaskConsumerApi ExternalTaskApi { get; }

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

        public async Task<ProcessStartResponse<object>> StartProcessInstance(
            string processModelId,
            string startEventId,
            string endEventId = "")
        {
            var request = new ProcessStartRequest<object>();

            return await this.StartProcessInstance<object, object>(processModelId, startEventId, request, endEventId);
        }

        public async Task<ProcessStartResponse<TResponsePayload>> StartProcessInstance<TResponsePayload>(
            string processModelId,
            string startEventId,
            string endEventId = "")
        where TResponsePayload: new()
        {
            var request = new ProcessStartRequest<object>();

            return await this.StartProcessInstance<object, TResponsePayload>(processModelId, startEventId, request, endEventId);
        }

        public async Task<ProcessStartResponse<TResponsePayload>> StartProcessInstance<TRequestPayload, TResponsePayload>(
            string processModelId,
            string startEventId,
            ProcessStartRequest<TRequestPayload> request,
            string endEventId = "")
        where TRequestPayload : new()
        where TResponsePayload : new()
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

        public async Task SubscribeToExternalTasksWithTopic<TPayload, TResult>(
            string topic,
            int maxTasks,
            int timeout,
            HandleExternalTaskAction<TPayload, TResult> handleAction)
        where TPayload : new()
        {
            var externalTaskWorker = new ProcessEngine.ExternalTaskAPI.Client.ExternalTaskWorker<TPayload, TResult>(
                this.HttpClient.BaseAddress.ToString(),
                this.Identity.ExternalTaskIdentity, topic, maxTasks, timeout, handleAction);

            externalTaskWorker.Start();

        }

        public async Task SubscribeToExternalTasksWithTopic<TPayload, TResult>(
            string topic,
            HandleExternalTaskAction<TPayload, TResult> handleAction)
        where TPayload : new()
        {
            var maxTasks = 10;
            var timeout = 1000;

            await this.SubscribeToExternalTasksWithTopic(topic, maxTasks, timeout, handleAction);
        }
    }
}

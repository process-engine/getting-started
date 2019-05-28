namespace ProcessEngineClient
{
    using global::ProcessEngine.ConsumerAPI.Client;
    using global::ProcessEngine.ConsumerAPI.Contracts;
    using global::ProcessEngine.ConsumerAPI.Contracts.DataModel;

    public class ProcessStartRequest<TPayload>
        where TPayload: new()
    {
        public string CorrelationId { get; set; }

        public string ParentProcessInstanceId { get; set; }


        public TPayload Payload { get; set;}

        public ProcessStartRequest()
        {
            this.Payload = new TPayload();
        }
    }    
}

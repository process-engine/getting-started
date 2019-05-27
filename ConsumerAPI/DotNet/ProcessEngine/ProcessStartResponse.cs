namespace LagerTeilautomatisch.ProcessEngine
{
    using global::ProcessEngine.ConsumerAPI.Client;
    using global::ProcessEngine.ConsumerAPI.Contracts;
    using global::ProcessEngine.ConsumerAPI.Contracts.DataModel;

    public class ProcessStartResponse<TResponsePayload>
        where TResponsePayload: new()
    {
        public string ProcessInstanceId { get; set; }

        public string CorrelationId { get; set; }

        public string EndEventId { get; set; }

        public TResponsePayload Payload { get; set; }

        public ProcessStartResponse() 
        {
            this.Payload = new TResponsePayload();
        }
    }
}
const {HttpClient} = require('@essential-projects/http');

const {ConsumerApiClientService, ExternalAccessor} = require('@process-engine/consumer_api_client');
const {DataModels} = require('@process-engine/consumer_api_contracts');

const PROCESS_ENGINE_BASE_URL = 'http://localhost:8000';

const identity = {
    token: 'ZHVtbXlfdG9rZW4=',
};

const PROCESS_MODEL_ID= "Lager-Teilautomatisch";

const START_EVENT_ID = "VersandauftragErhalten";
const END_EVENT_ID = "VersandauftragVersendet";

async function main() {
    let result;

    const httpClient = new HttpClient();
    httpClient.config = {url: PROCESS_ENGINE_BASE_URL};

    const externalAccessor = new ExternalAccessor(httpClient);
    const client = new ConsumerApiClientService(externalAccessor);

    const processStartPayload = new DataModels.ProcessModels.ProcessStartRequestPayload();

    console.log(`Prozess gestartet '${PROCESS_MODEL_ID}' beim Start-Event '{START_EVENT_ID}'.`);
    
    result = await client.startProcessInstance(
        identity,
        PROCESS_MODEL_ID,
        processStartPayload,
        DataModels.ProcessModels.StartCallbackType.CallbackOnEndEventReached,
        START_EVENT_ID,
        END_EVENT_ID);

    console.log(`Prozess beendet (CorrelationId: '${result.correlationId}').`);
    console.log("Daten: ");
    console.log(result.tokenPayload);
}

main();
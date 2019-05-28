const {HttpClient} = require('@essential-projects/http');

const {ConsumerApiClientService, ExternalAccessor} = require('@process-engine/consumer_api_client');
const {DataModels} = require('@process-engine/consumer_api_contracts');

const identity = {
    token: 'ZHVtbXlfdG9rZW4=',
};

const PROCESS_MODEL_ID= 'Benutzeraktivierung';

const START_EVENT_ID = 'StartAktivierung';
const END_EVENT_ID = 'EndeAktivierung';

function createConsumerClient(url) {
    const httpClient = new HttpClient();
    httpClient.config = {url: url};

    const externalAccessor = new ExternalAccessor(httpClient);
    const client = new ConsumerApiClientService(externalAccessor);

    return client;
}

function createPayload(shoppingCardAmount) {
    const processStartPayload = new DataModels.ProcessModels.ProcessStartRequestPayload();
    processStartPayload.inputValues = {shoppingCardAmount: shoppingCardAmount};

    return processStartPayload;
}

async function main() {
    const client = createConsumerClient('http://localhost:8000');

    const processStartPayload = createPayload(1000);

    console.log(`Prozess gestartet '${PROCESS_MODEL_ID}' beim Start-Event '${START_EVENT_ID}'.`);
    
    const result = await client.startProcessInstance(
        identity,
        PROCESS_MODEL_ID,
        processStartPayload,
        DataModels.ProcessModels.StartCallbackType.CallbackOnEndEventReached,
        START_EVENT_ID,
        END_EVENT_ID);

    console.log(`Prozess beendet (CorrelationId: '${result.correlationId}').`);
    console.log('Daten: ');
    console.log(result.tokenPayload);
}

main();
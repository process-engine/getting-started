const {HttpClient} = require('@essential-projects/http');

const {ConsumerApiClientService, ExternalAccessor} = require('@process-engine/consumer_api_client');

const PROCESS_ENGINE_BASE_URL = 'http://localhost:8000';

const identity = {
    token: 'ZHVtbXlfdG9rZW4=',
};

const PROCESS_MODEL_ID= "Lager-Teilautomatisch";

const START_EVENT_ID = "VersandauftragErhalten";

async function main() {
    let result;

    const httpClient = new HttpClient();
    httpClient.config = {url: PROCESS_ENGINE_BASE_URL};

    const externalAccessor = new ExternalAccessor(httpClient);
    const client = new ConsumerApiClientService(externalAccessor);

    const processStartPayload = new 

    result = await client.startProcessInstance(
        identity,
        PROCESS_MODEL_ID,
        processStartPayload);
}

main();
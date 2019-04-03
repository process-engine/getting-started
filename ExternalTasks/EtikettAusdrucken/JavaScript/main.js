const {HttpClient} = require('@essential-projects/http');
const {
  ExternalTaskApiClientService,
  ExternalTaskApiExternalAccessor,
  ExternalTaskWorker,
} = require('@process-engine/external_task_api_client');

const {
  ExternalTaskFinished,
} = require('@process-engine/external_task_api_contracts');

const identity = {
    token: 'ZHVtbXlfdG9rZW4=',
};

const PROCESS_ENGINE_BASE_URL = 'http://localhost:8000';
const TOPIC = 'Etikett-ausdrucken';
const MAX_TASKS = 10;
const POLLING_TIMEOUT = 1000;
const WAIT_TIMEOUT = 10000;

const handleTaskCallback = async (externalTask) => {
    console.log("");
    console.log(externalTask);
    console.log("");

    console.log(`Warte für ${WAIT_TIMEOUT} Millisekunden.`);
    await sleep(WAIT_TIMEOUT);

    const result = { 
        TestProperty: 'Fertig'
    };

    console.log("Bearbeitung fertig!");
    return new ExternalTaskFinished(externalTask.id, result);
};

async function main() {
    const httpClient = new HttpClient();
    httpClient.config = {url: PROCESS_ENGINE_BASE_URL};
    
    const externalAccessor = new ExternalTaskApiExternalAccessor(httpClient);
    const externalTaskAPIService = new ExternalTaskApiClientService(externalAccessor);
    const externalTaskWorker = new ExternalTaskWorker(externalTaskAPIService);

    console.log(`Warten auf Aufgaben für das Topic '${TOPIC}'.`);

    externalTaskWorker.waitForAndHandle(
        identity,
        TOPIC,
        MAX_TASKS,
        POLLING_TIMEOUT,
        handleTaskCallback
    );
}

async function sleep(milliseconds) {
    return new Promise((resolve) => {
        setTimeout(() => resolve(), milliseconds);
    });
}

main();
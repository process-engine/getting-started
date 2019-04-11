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

const TOPIC = 'Etikett-ausdrucken';
const MAX_TASKS = 10;
const POLLING_TIMEOUT = 1000;
const WAIT_TIMEOUT = 10000;

function createExternalTaskWorker(url) {
    const httpClient = new HttpClient();
    httpClient.config = {url: url};
    
    const externalAccessor = new ExternalTaskApiExternalAccessor(httpClient);

    const externalTaskAPIService = new ExternalTaskApiClientService(externalAccessor);

    const externalTaskWorker = new ExternalTaskWorker(externalTaskAPIService);

    return externalTaskWorker;
}

const doSomeLongWork = async (externalTask) => {

    console.log(`Warte für ${WAIT_TIMEOUT} Millisekunden.`);
    await sleep(WAIT_TIMEOUT);

    const result = { 
        TestProperty: 'Dies ist das Ergebnis vom JavaScript-external-Task.'
    };

    console.log('Bearbeitung fertig!');

    return result;
};

async function main() {
    const externalTaskWorker = createExternalTaskWorker('http://localhost:8000');

    console.log(`Warten auf Aufgaben für das Topic '${TOPIC}'.`);

    externalTaskWorker.waitForAndHandle(identity, TOPIC, MAX_TASKS, POLLING_TIMEOUT, async (externalTask) => {
        console.log('Daten external-Task: ');
        console.log(externalTask);
        console.log('');

        let result = await doSomeLongWork();

        let externalTaskFinished = new ExternalTaskFinished(externalTask.id, result);

        return externalTaskFinished;
    }); 
}

async function sleep(milliseconds) {
    return new Promise((resolve) => setTimeout(resolve, milliseconds));
}

main();
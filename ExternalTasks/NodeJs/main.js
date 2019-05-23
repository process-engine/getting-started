const {HttpClient} = require('@essential-projects/http');

const {
  ExternalTaskApiClientService,
  ExternalTaskApiExternalAccessor,
  ExternalTaskWorker,
} = require('@process-engine/external_task_api_client');
const {ExternalTaskFinished} = require('@process-engine/external_task_api_contracts');

const identity = {
  token: 'ZHVtbXlfdG9rZW4=',
};

async function main() {
  const externalTaskWorker = createExternalTaskWorker('http://localhost:8000');

  const topic = 'Etikett-ausdrucken';
  const maxTasks = 10;
  const pollingTimeout = 1000;

  console.log(`Warten auf Aufgaben für das Topic '${topic}'.`);

  externalTaskWorker.waitForAndHandle(identity, topic, maxTasks, pollingTimeout, async (externalTask) => {
    console.log('Daten external-Task: ');
    console.log(externalTask);
    console.log('');

    let result = await doSomeLongWork();

    let externalTaskFinished = new ExternalTaskFinished(externalTask.id, result);

    return externalTaskFinished;
  });
}

function createExternalTaskWorker(url) {
  const httpClient = new HttpClient();
  httpClient.config = {url: url};

  const externalAccessor = new ExternalTaskApiExternalAccessor(httpClient);

  const externalTaskAPIService = new ExternalTaskApiClientService(externalAccessor);

  const externalTaskWorker = new ExternalTaskWorker(externalTaskAPIService);

  return externalTaskWorker;
}

const doSomeLongWork = async (externalTask) => {

  const longWorkTimeout = 10000;
  console.log(`Warte für ${longWorkTimeout} Millisekunden.`);
  await sleep(longWorkTimeout);

  const result = {
      TestProperty: 'Dies ist das Ergebnis vom NodeJS-external-Task.'
  };

  console.log('ExternalTask erfolgreich bearbeitet!');

  return result;
};

async function sleep(milliseconds) {
  return new Promise((resolve) => setTimeout(resolve, milliseconds));
}

main();

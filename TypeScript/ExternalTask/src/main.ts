import {IIdentity} from '@essential-projects/iam_contracts';

import {ExternalTaskWorker} from '@process-engine/consumer_api_client'

import {DataModels} from '@process-engine/consumer_api_contracts'

const identity: IIdentity = {
  token: 'ZHVtbXlfdG9rZW4=',
  userId: 'dummy_token'
};

const topic = 'AktivierungsemailSenden';
const maxTasks = 10;
const longPollingTimeout = 1000;

type MyExternalTaskPayload = {
  shoppingCardAmount: number;
}

type MyExternalTaskResult = {
  shoppingCardAmount: number;
}

async function main() {
  const externalTaskWorker = createExternalTaskWorker('http://localhost:8000');

  console.log(`Warten auf Aufgaben für das Topic '${topic}'.`);
  externalTaskWorker.start();
}

function createExternalTaskWorker(url) {

  const externalTaskWorker = new ExternalTaskWorker<MyExternalTaskPayload, MyExternalTaskResult>(
    url,
    identity,
    topic,
    maxTasks,
    longPollingTimeout,
    doSomeLongWork,
  );

  return externalTaskWorker;
}

async function doSomeLongWork(externalTask: DataModels.ExternalTask.ExternalTask<MyExternalTaskPayload>): Promise<DataModels.ExternalTask.ExternalTaskResultBase> {

  const simulateWorkTimeout = 10000;

  console.log(`Warte für ${simulateWorkTimeout} Millisekunden.`);
  await sleep(simulateWorkTimeout);

  const resultPayload = {
    shoppingCardAmount: externalTask.payload.shoppingCardAmount
  };

  const result = new DataModels.ExternalTask.ExternalTaskSuccessResult(externalTask.id, resultPayload);

  console.log('Bearbeitung fertig!');

  return result;
};

async function sleep(milliseconds) {
  return new Promise((resolve) => setTimeout(resolve, milliseconds));
}

main();

import {HttpClient} from '@essential-projects/http';
import {IIdentity} from '@essential-projects/iam_contracts';

import {ConsumerApiClient, ExternalAccessor} from '@process-engine/consumer_api_client'
import {DataModels} from '@process-engine/consumer_api_contracts'

const identity: IIdentity = {
  token: 'ZHVtbXlfdG9rZW4=',
  userId: 'dummy_token'
};

const processModelId = 'Benutzeraktivierung';

const startEventId = 'StartAktivierung';
const endEventId = 'EndeAktivierung';

async function main(): Promise<void> {
  const client = createConsumerApiClient('http://localhost:8000');

  const processStartPayload = createProcessStartPayload(1000);

  console.log(`Prozess '${processModelId}' mit Start Event '${startEventId}' gestartet`);

  const result = await client.startProcessInstance(
    identity,
    processModelId,
    processStartPayload,
    DataModels.ProcessModels.StartCallbackType.CallbackOnEndEventReached,
    startEventId,
    endEventId,
  );

  console.log('Prozess beendet');
  console.log('Correlation ID: ', result.correlationId)
  console.log('Ergebnis: ');
  console.log(result.tokenPayload);
}

function createConsumerApiClient(url: string): ConsumerApiClient {
  const httpClient = new HttpClient();
  httpClient.config = {url: url};

  const externalAccessor = new ExternalAccessor(httpClient);
  const client = new ConsumerApiClient(externalAccessor);

  return client;
}

function createProcessStartPayload(shoppingCardAmount: number): DataModels.ProcessModels.ProcessStartRequestPayload {
  const processStartPayload = new DataModels.ProcessModels.ProcessStartRequestPayload();
  processStartPayload.inputValues = {
    shoppingCardAmount: shoppingCardAmount,
  };

  return processStartPayload;
}

main();

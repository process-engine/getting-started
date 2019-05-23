const {HttpClient} = require('@essential-projects/http');

const {ConsumerApiClientService, ExternalAccessor} = require('@process-engine/consumer_api_client');
const {DataModels} = require('@process-engine/consumer_api_contracts');

const sampleIdentity = {
  token: 'ZHVtbXlfdG9rZW4=',
};

const processModelId= 'Lager-Manuell';

const startEventId = 'VersandauftragErhalten';
const endEventId = 'VersandauftragVersendet';

async function main() {
  const client = createConsumerApiClient('http://localhost:8000');

  const processStartPayload = createStartRequestPayload('Dies ist die Eingabe für den Prozess aus JavaScript.');

  console.log(`Prozess gestartet '${processModelId}' beim Start-Event '${startEventId}'.`);

  const processStartResult = await client.startProcessInstance(
    sampleIdentity,
    processModelId,
    processStartPayload,
    DataModels.ProcessModels.StartCallbackType.CallbackOnEndEventReached,
    startEventId,
    endEventId,
  );

  console.log(`Prozess beendet (CorrelationId: '${processStartResult.correlationId}').`);
  console.log('Daten: ');
  console.log(processStartResult.tokenPayload);
}

function createConsumerApiClient(url) {
  const httpClient = new HttpClient();
  httpClient.config = {url: url};

  const externalAccessor = new ExternalAccessor(httpClient);
  const client = new ConsumerApiClientService(externalAccessor);

  return client;
}

function createStartRequestPayload(inputProperty) {
  const processStartPayload = new DataModels.ProcessModels.ProcessStartRequestPayload();
  processStartPayload.inputValues = {InputProperty: inputProperty};

  return processStartPayload;
}

main();

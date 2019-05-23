
# Einstieg in die Verwendung der Komponenten aus 'process-engine.io'

## Was sind die Ziele dieses Projekts?

* Starten einer neuen Prozessinstanz mittels Consumer-API
* Ausführung von Aktivitäten mittels External-Task-Pattern

Anhand zweier Beispiele in Node.js und .NET (Core) wird die Verwendung dargestellt.

## Wie kann ich die Beispiele verwenden?

### Voraussetzungen

* [BPMN-Studio](http:///www.process-engine.io)
* [.NET Core 2.1 SDK](https://www.microsoft.com/net/download/core) - die
 `dotnet` CLI muss über die Konsole/das Terminal ausführbar sein
* [NodeJS](https://nodejs.org/en/) `>= v8.11.*`und npm `>= 5.6.*` - `npm` muss über die Konsole/das Terminal ausführbar sein.

### Setup/Installation

* Die BPMN-Diagramme im Verzeichnis *Prozesse* müssen in der ProcessEngine, die mit dem BPMN-Studio geliefert wird, deployed sein.

## Wie kann ich das Projekt benutzen?

* Das BPMN-Studio starten
    * Die lokale ProcessEngine wird automatisch unter http://localhost:8000 mit gestartet.

### Prozess starten

#### Prozess starten mit .NET

* `cd ConsumerAPI/DotNet`
* `dotnet run`
* BPMN-Studio öffnen und:

   * zur lokalen ProcessEngine navigieren (http://localhost:8000)
   * Prozess *Lager-Manuell* auswählen
   * zur Ansicht *Inspect* wechseln
   * unter *Processes running* auf *Live Execution Tracker* wechseln
   * von hieraus kann der Prozess manuell fortgeführt werden

#### Prozess starten mit NodeJS

* `cd ConsumerAPI/NodeJs`
* `npm install`
* `npm start`
* BPMN-Studio öffnen und:

   * zur lokalen ProcessEngine navigieren (http://localhost:8000)
   * Prozess *Lager-Manuell* auswählen
   * Zur Ansicht *Inspect* wechseln
   * Unter *Processes running* auf *Live Execution Tracker* wechseln
   * von hieraus kann der Prozess manuell fortgeführt werden

### Tasks mit dem *External-Task-Pattern* automatisieren

#### *External-Task-Pattern* mit .NET

* Änderungen im Code ausführen:
    *  ExternalTasks/DotNet/Programm.cs Zeile 17 auskommentieren und Zeile 18 einkommentieren, so
       dass `string PROCESS_MODEL_ID= "Lager-Teilautomatisch"` verwendet wird.
* `cd ExternalTasks/DotNet`
* `dotnet run`
* BPMN-Studio öffnen und:
   * zur lokalen ProcessEngine navigieren (http://localhost:8000)
   * Prozess *Lager-Teilautomatisch* auswählen
   * Prozess starten und manuell ausführen
   * Der Task *Etikett ausdrucken*  wird durch das .NET-Programm automatisch weitergeführt
   * Anschließend kann der Prozess manuell bis zum End-Event ausgeführt werden

#### *External-Task-Pattern* mit NodeJS

* Änderungen im Code ausführen:
    *  ExternalTasks/DotNet/Programm.cs Zeile 17 auskommentieren und Zeile 18 einkommentieren, so
       dass `string PROCESS_MODEL_ID= "Lager-Teilautomatisch"` verwendet wird.
* `cd ConsumerAPI/NodeJs`
* `npm install`
* `npm start`
* BPMN-Studio öffnen und:
   * zur lokalen ProcessEngine navigieren (http://localhost:8000)
   * Prozess *Lager-Teilautomatisch* auswählen
   * Prozess starten und manuell ausführen
   * Der Task *Etikett ausdrucken*  wird durch das NodeJS-Programm automatisch weitergeführt
   * Anschließend kann der Prozess manuell bis zum End-Event ausgeführt werden

### Wen kann ich auf das Projekt ansprechen?

- René Föhring (@rrrene)
- Martin Möllenbeck (@moellenbeck)

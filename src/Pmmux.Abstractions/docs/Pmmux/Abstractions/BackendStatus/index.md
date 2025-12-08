#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## BackendStatus Enum

Health status of a backend as tracked by [IBackendMonitor](../IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor')\.

```csharp
public enum BackendStatus
```
### Fields

<a name='Pmmux.Abstractions.BackendStatus.Unknown'></a>

`Unknown` 0

Backend's health status is unknown \(no health checks performed yet\)\.

### Remarks
Backends start in this state\. Most routing strategies treat unknown as healthy\.

<a name='Pmmux.Abstractions.BackendStatus.Healthy'></a>

`Healthy` 1

Backend is healthy and available for routing traffic\.

<a name='Pmmux.Abstractions.BackendStatus.Unhealthy'></a>

`Unhealthy` 2

Backend is unhealthy and should not receive new traffic\.

### Remarks
Backends enter this state after consecutive health check failures\.

<a name='Pmmux.Abstractions.BackendStatus.Degraded'></a>

`Degraded` 3

Backend is partially healthy and may handle traffic with reduced reliability\.

### Remarks
Use with caution\. Consider preferring fully healthy backends when available\.

<a name='Pmmux.Abstractions.BackendStatus.Stabilizing'></a>

`Stabilizing` 4

Backend is recovering from an unhealthy state\.

### Remarks
Backends enter this state after passing a health check while unhealthy\.
They return to healthy after consecutive successful checks\.

<a name='Pmmux.Abstractions.BackendStatus.Draining'></a>

`Draining` 5

Backend is draining active connections and should not receive new traffic\.

### Remarks
Used during graceful shutdown\. Existing connections are allowed to complete\.

<a name='Pmmux.Abstractions.BackendStatus.Stopped'></a>

`Stopped` 6

Backend has stopped and is not available\.

### Remarks
Backend status indicates the current operational state and availability\.
The [IRoutingStrategy](../IRoutingStrategy/index.md 'Pmmux\.Abstractions\.IRoutingStrategy') uses this status to decide which backends
should receive traffic\. Status transitions are managed by the backend monitor
based on health check results and configured thresholds\.
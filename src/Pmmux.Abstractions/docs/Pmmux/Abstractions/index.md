#### [Pmmux\.Abstractions](../../index.md 'index')

## Pmmux\.Abstractions Namespace

| Classes | |
| :--- | :--- |
| [BackendInfo](BackendInfo/index.md 'Pmmux\.Abstractions\.BackendInfo') | Represents runtime information about an active backend instance\. |
| [BackendSpec](BackendSpec/index.md 'Pmmux\.Abstractions\.BackendSpec') | Specification for creating a backend instance\. |
| [BackendStatusInfo](BackendStatusInfo/index.md 'Pmmux\.Abstractions\.BackendStatusInfo') | Health status and statistics for a backend\. |
| [ClientConnectionContext](ClientConnectionContext/index.md 'Pmmux\.Abstractions\.ClientConnectionContext') | Context for client connection negotiators, containing connection details and properties\. |
| [ClientInfo](ClientInfo/index.md 'Pmmux\.Abstractions\.ClientInfo') | Endpoint information for a remote client\. |
| [CompactConfig](CompactConfig/index.md 'Pmmux\.Abstractions\.CompactConfig') | Utility for parsing compact configuration strings\. |
| [CompactConfig\.IdentifierSegment](CompactConfig/IdentifierSegment/index.md 'Pmmux\.Abstractions\.CompactConfig\.IdentifierSegment') | Identifier segment\. |
| [CompactConfig\.PropertiesSegment](CompactConfig/PropertiesSegment/index.md 'Pmmux\.Abstractions\.CompactConfig\.PropertiesSegment') | Properties segment \(key\-value pairs\) in the configuration\. |
| [CompactConfig\.Segment](CompactConfig/Segment/index.md 'Pmmux\.Abstractions\.CompactConfig\.Segment') | Base type for segments\. |
| [CounterMetric](CounterMetric/index.md 'Pmmux\.Abstractions\.CounterMetric') | Counter metric that tracks cumulative values\. |
| [DurationMetric](DurationMetric/index.md 'Pmmux\.Abstractions\.DurationMetric') | Duration metric that tracks time\-based measurements\. |
| [GaugeMetric](GaugeMetric/index.md 'Pmmux\.Abstractions\.GaugeMetric') | Gauge metric that tracks instantaneous values\. |
| [HealthCheckResult](HealthCheckResult/index.md 'Pmmux\.Abstractions\.HealthCheckResult') | Result of a health check operation\. |
| [HealthCheckSpec](HealthCheckSpec/index.md 'Pmmux\.Abstractions\.HealthCheckSpec') | Specifies how to perform health checks on backends\. |
| [IClientConnectionNegotiator\.Result](IClientConnectionNegotiator/Result/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator\.Result') | Result of a connection negotiation attempt\. |
| [IRouter\.Result](IRouter/Result/index.md 'Pmmux\.Abstractions\.IRouter\.Result') | Result of a routing operation\. |
| [ListenerInfo](ListenerInfo/index.md 'Pmmux\.Abstractions\.ListenerInfo') | Information about a network listener and its accessibility\. |
| [Metric](Metric/index.md 'Pmmux\.Abstractions\.Metric') | Base class for all metric types\. |
| [Metric&lt;T&gt;](Metric_T_/index.md 'Pmmux\.Abstractions\.Metric\<T\>') | Base class for metrics with a specific value type\. |
| [NatDeviceInfo](NatDeviceInfo/index.md 'Pmmux\.Abstractions\.NatDeviceInfo') | Information about a discovered NAT device on the network\. |
| [PortMapInfo](PortMapInfo/index.md 'Pmmux\.Abstractions\.PortMapInfo') | Represents information about a NAT port mapping between a public endpoint and a local port\. |

| Interfaces | |
| :--- | :--- |
| [IBackend](IBackend/index.md 'Pmmux\.Abstractions\.IBackend') | Base interface for all backend implementations that handle traffic in the port multiplexer\. |
| [IBackendMonitor](IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor') | Monitors backend health status based on configured health check specifications\. |
| [IBackendProtocol](IBackendProtocol/index.md 'Pmmux\.Abstractions\.IBackendProtocol') | Factory for creating backend instances from specifications\. |
| [IClientConnection](IClientConnection/index.md 'Pmmux\.Abstractions\.IClientConnection') | Established client connection ready for routing and data transfer\. |
| [IClientConnectionNegotiator](IClientConnectionNegotiator/index.md 'Pmmux\.Abstractions\.IClientConnectionNegotiator') | Negotiates client connections before routing using a chain\-of\-responsibility pattern\. |
| [IClientConnectionPreview](IClientConnectionPreview/index.md 'Pmmux\.Abstractions\.IClientConnectionPreview') | Read\-only preview of a client connection for making routing decisions without consuming data\. |
| [IClientWriter](IClientWriter/index.md 'Pmmux\.Abstractions\.IClientWriter') | Writes data to a client via a listener\. |
| [IClientWriterFactory](IClientWriterFactory/index.md 'Pmmux\.Abstractions\.IClientWriterFactory') | Factory to create [IClientWriter](IClientWriter/index.md 'Pmmux\.Abstractions\.IClientWriter') instances\. |
| [ICommandLineBuilder](ICommandLineBuilder/index.md 'Pmmux\.Abstractions\.ICommandLineBuilder') | Registers command\-line options with the [System\.CommandLine](https://learn.microsoft.com/en-us/dotnet/api/system.commandline 'System\.CommandLine') parser\. |
| [IConnection](IConnection/index.md 'Pmmux\.Abstractions\.IConnection') | Bidirectional data connection using [System\.IO\.Pipelines](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipelines 'System\.IO\.Pipelines')\. |
| [IConnectionlessBackend](IConnectionlessBackend/index.md 'Pmmux\.Abstractions\.IConnectionlessBackend') | Backend for connectionless protocols like UDP\. |
| [IConnectionOrientedBackend](IConnectionOrientedBackend/index.md 'Pmmux\.Abstractions\.IConnectionOrientedBackend') | Backend for connection\-oriented protocols like TCP\. |
| [IEventNotifier](IEventNotifier/index.md 'Pmmux\.Abstractions\.IEventNotifier') | Allows components to subscribe to events for system changes\. |
| [IEventSender](IEventSender/index.md 'Pmmux\.Abstractions\.IEventSender') | Raises events for system changes\. |
| [IExtension](IExtension/index.md 'Pmmux\.Abstractions\.IExtension') | Plugin for extending the port multiplexer with additional capabilities\. |
| [IHealthCheckBackend](IHealthCheckBackend/index.md 'Pmmux\.Abstractions\.IHealthCheckBackend') | Optional backend interface for health checking support\. |
| [IMetricReporter](IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter') | Reports metrics from port multiplexer components\. |
| [IMetricSink](IMetricSink/index.md 'Pmmux\.Abstractions\.IMetricSink') | Receives metrics from the [IMetricReporter](IMetricReporter/index.md 'Pmmux\.Abstractions\.IMetricReporter')\. |
| [IPortMultiplexer](IPortMultiplexer/index.md 'Pmmux\.Abstractions\.IPortMultiplexer') | Main orchestration interface for the port multiplexer\. |
| [IPortWarden](IPortWarden/index.md 'Pmmux\.Abstractions\.IPortWarden') | Manages NAT port mappings \(UPnP/PMP\)\. |
| [IRouter](IRouter/index.md 'Pmmux\.Abstractions\.IRouter') | Routes client traffic to backends and manages backend lifecycle\. |
| [IRoutingStrategy](IRoutingStrategy/index.md 'Pmmux\.Abstractions\.IRoutingStrategy') | Strategy for selecting a backend from multiple candidates during routing\. |

| Enums | |
| :--- | :--- |
| [BackendStatus](BackendStatus/index.md 'Pmmux\.Abstractions\.BackendStatus') | Health status of a backend as tracked by [IBackendMonitor](IBackendMonitor/index.md 'Pmmux\.Abstractions\.IBackendMonitor')\. |
| [PriorityTier](PriorityTier/index.md 'Pmmux\.Abstractions\.PriorityTier') | Routing priority tier for backends\. |

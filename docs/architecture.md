# pmmux System Architecture

pmmux is a port-forwarding traffic multiplexer that accepts client connections on TCP/UDP ports and routes them to backend servers based on content inspection, connection properties, and configurable selection algorithms. The system uses an asynchronous, pipeline-based architecture for high-performance data transfer.

## Technical Foundation

- Dual transport protocol support (TCP/UDP)
- Pluggable application-layer backend protocols
- Configurable routing strategies
- Health monitoring with failover
- Extension system for custom functionality

## Component Architecture

### Port Multiplexer

The port multiplexer is the entry point for all network traffic. It binds to configured ports, coordinates port forwarding with NAT devices, and hands off established connections to the router.

### Router

The router orchestrates client traffic with backends and manages backend lifecycle. It coordinates connection negotiation, enumerates matching backends based on connection properties, delegates selection to the routing strategy, and manages backend instances.

#### TCP Routing
1. Receive client connection from port multiplexer
2. Pass connection through negotiator chain for transport-level processing
3. Enumerate matching backends in definition order, allowing each to inspect connection via preview without consuming data
4. Delegate to routing strategy for final backend selection
5. Selected backend relays traffic to upstream server

#### UDP Routing
1. Receive message from port multiplexer
2. Enumerate matching backends in definition order, allowing each to inspect message data
3. Delegate to routing strategy for final backend selection
4. Selected backend processes the message

### Backends

Backends process client requests and come in two types based on network protocol.

#### Connection-Oriented (TCP)

Inspect connection properties and preview data to determine if they can handle the connection, then relay traffic bidirectionally between client and backend. For example, the passthrough backend forwards traffic to a configured upstream TCP endpoint.

#### Connectionless (UDP)

Inspect message data to determine if they can handle a message, then process it accordingly.

### Connection Negotiation

Connection negotiators intercept connections before routing for transport-level protocol processing. Multiple negotiators can be chained together to perform protocol handshakes, extract connection properties, and transform the connection stream.

For example, the [TLS extension](../src/Pmmux.Extensions.Tls/README.md) provides a negotiator that performs TLS termination and extracts properties like SNI hostname, ALPN protocol, and cipher suite. These properties are then available to backends for content-based routing.

### Client Connection Architecture

Client connections use a duplex stream with a preview mechanism to enable protocol detection.

#### Preview Mechanism

The preview system buffers initial connection data so multiple backends can inspect it non-destructively during routing. Backends parse protocol data to determine if they can handle the connection. The buffered data is preserved and replayed when the connection starts, enabling content-based routing without protocol-specific logic in the router. Buffer size can be limited via `--preview-buffer-limit`.

#### Connection Properties

Connection properties are extracted from socket metadata (remote IP, port, protocol). Connection negotiators can augment these during negotiation. Backends use properties for routing decisions and traffic handling.

### Routing Strategies

Routing strategies are pluggable algorithms for backend selection when multiple backends match a connection. Strategies receive matching backends (already filtered by health status) and select one. Backends are evaluated lazily, allowing strategies to terminate early without checking all candidates. Backend selection has a timeout (`--selection-timeout`, default 5000ms) to prevent resource exhaustion from slow clients.

#### Built-in Strategies

- **first-available**: Returns the first matched backend in configuration order. Does not consider priority tiers or load metrics.
- **least-requests**: Selects the backend with the fewest routed requests. Evaluates backends by priority tier first, only considering lower tiers if higher tiers are unavailable.

Custom routing strategies can be built as extensions.

### Health Monitoring

The health monitoring system provides automatic health checking with configurable thresholds and failover. Backends that support health checking are monitored continuously on configured intervals with timeout protection. Status is determined based on consecutive failure and recovery thresholds to prevent flapping.

#### Configuration

- Protocol and backend targeting (specific or all)
- Interval between checks (default 10s)
- Check timeout (default 5s)
- Initial delay before first check
- Failure threshold for marking unhealthy (default 3)
- Recovery threshold for marking healthy (default 5)

The monitor runs health checks on each interval, tracks consecutive failures and successes, and updates backend status based on configured thresholds. Unhealthy backends are excluded from routing.

#### Health Status States

- **Unknown**: No health checks performed yet. Most routing strategies treat this as healthy.
- **Healthy**: Backend is available for routing traffic.
- **Unhealthy**: Backend has failed consecutive health checks and should not receive new traffic.
- **Degraded**: Backend is partially healthy and may handle traffic with reduced reliability.
- **Stabilizing**: Backend is recovering from unhealthy state after passing a health check.
- **Draining**: Backend is shutting down gracefully; existing connections can complete but no new traffic is routed.
- **Stopped**: Backend has stopped and is not available.

### Priority Tiers

Backends are organized into priority tiers for failover and traffic management.

#### Tiers (Highest to Lowest)
1. **vip**: Premium/primary servers
2. **normal**: Standard production backends (default)
3. **standby**: Backup/DR resources
4. **fallback**: Error pages, blackholes

Priority tier may inform the decision of the routing strategy, but not all routing strategies use priority tier as selection criteria.

### Extension System

The extension system provides multiple extension points for customization. Extensions can add new components or replace existing ones by registering alternative service implementations. Extensions have full access to the dependency injection container and can replace almost any component, including core routing logic, health monitoring, and connection handling. While standard extension points cover most use cases, the architecture allows for deep customization when needed.

## See Also

- [Configuration Guide](configuration.md)
- [Extensibility Guide](extensibility.md)
- [API Reference](api-reference.md)

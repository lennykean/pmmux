using System.IO.Pipelines;

namespace Pmmux.Extensions.Http;

/// <summary>Duplex pipe adapter for pmmux connections.</summary>
public record PmmuxDuplexPipe(PipeReader Input, PipeWriter Output) : IDuplexPipe;

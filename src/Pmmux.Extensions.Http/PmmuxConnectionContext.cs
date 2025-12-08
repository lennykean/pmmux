using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Features;

using Pmmux.Abstractions;

namespace Pmmux.Extensions.Http;

internal class PmmuxConnectionContext(ClientInfo client, PipeReader input, PipeWriter output) : ConnectionContext
{
    private readonly PipeReader _input = input;
    private readonly PipeWriter _output = output;
    private readonly CancellationTokenSource _connectionClosed = new();

    public override IDuplexPipe Transport { get; set; } = new PmmuxDuplexPipe(input, output);
    public override string ConnectionId { get; set; } = Guid.NewGuid().ToString();
    public override IFeatureCollection Features { get; } = new FeatureCollection();
    public override IDictionary<object, object?> Items { get; set; } = new Dictionary<object, object?>();
    public override EndPoint? RemoteEndPoint { get; set; } = client.RemoteEndpoint;
    public override CancellationToken ConnectionClosed => _connectionClosed.Token;

    public override async ValueTask DisposeAsync()
    {
        try
        {
            _connectionClosed.Cancel();
        }
        catch
        {
        }

        try
        {
            await _input.CompleteAsync().ConfigureAwait(false);
        }
        catch
        {
        }

        try
        {
            await _output.CompleteAsync().ConfigureAwait(false);
        }
        catch
        {
        }

        _connectionClosed.Dispose();
    }

    public override void Abort(ConnectionAbortedException abortReason)
    {
        try
        {
            _connectionClosed.Cancel();
        }
        catch
        {
        }
    }
}

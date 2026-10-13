using System;
using System.Collections.Generic;
using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Moq;

using NUnit.Framework;

using Pmmux.Abstractions;
using Pmmux.Extensions.QBittorrent;

using QBittorrent.Client;

namespace Pmmux.Test.QBittorrent;

[TestFixture]
public class QBittorrentExtensionTests
{
    private IExtension _subject = null!;
    private Dictionary<string, string?> _settings = null!;

    [SetUp]
    public void SetUp()
    {
        _subject = new QBittorrentExtension();
        _settings = new Dictionary<string, string?>
        {
            ["pmmux:--qbittorrent-enable"] = "true"
        };
    }

    [TestCase("http://192.168.1.152/")]
    [TestCase("https://user:password@example.com/")]
    [TestCase("https://example.com/?secret=value")]
    [TestCase("file:///tmp/qbit")]
    public void RegisterServices_RejectsEndpoint_WhenTransportOrUrlIsUnsafe(string url)
    {
        _settings["pmmux:--qbittorrent-url"] = url;
        using var services = RegisterServices();

        Assert.Throws<ArgumentException>(() => services.GetRequiredService<QBittorrentConfig>());
    }

    [TestCase("?")]
    [TestCase("6881")]
    public void RegisterCommandOptions_ParsesPeerPort_WhenSpecified(string port)
    {
        var command = new RootCommand();
        var builder = new Mock<ICommandLineBuilder>();
        builder.Setup(b => b.Add(It.IsAny<Option>()))
            .Callback((Option option) => command.Add(option))
            .Returns(builder.Object);
        _subject.RegisterCommandOptions(builder.Object);

        var result = command.Parse(["--qbittorrent-peer-port", port]);

        Assert.That(result.Errors, Is.Empty);
        Assert.That(result.GetValue(((QBittorrentExtension)_subject).PeerPortOption), Is.EqualTo(port));
    }

    [Test]
    public void RegisterServices_UsesPackagedClient_WhenEnabled()
    {
        using var services = RegisterServices();

        var client = services.GetRequiredService<IQBittorrentClient>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(client, Is.TypeOf<QBittorrentClient>());
            Assert.That(client.Timeout, Is.EqualTo(TimeSpan.FromSeconds(10)));
            Assert.That(client.DefaultRequestHeaders.Referrer, Is.EqualTo(new Uri("http://127.0.0.1:18080/")));
        }
    }

    [Test]
    public void RegisterServices_UsesAutomaticPeerPort_WhenPeerPortIsOmitted()
    {
        using var services = RegisterServices();

        var config = services.GetRequiredService<QBittorrentConfig>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(config.PeerPort, Is.Null);
            Assert.That(config.Endpoint, Is.EqualTo(new Uri("http://127.0.0.1:18080/")));
            Assert.That(config.Interval, Is.EqualTo(TimeSpan.FromSeconds(30)));
        }
    }

    [TestCase("?", null)]
    [TestCase("1", 1)]
    [TestCase("36140", 36140)]
    [TestCase("65535", 65535)]
    public void RegisterServices_AcceptsPeerPort_WhenAutomaticOrInRange(string port, int? expected)
    {
        _settings["pmmux:--qbittorrent-peer-port"] = port;
        using var services = RegisterServices();

        Assert.That(services.GetRequiredService<QBittorrentConfig>().PeerPort, Is.EqualTo(expected));
    }

    [TestCase("0")]
    [TestCase("-1")]
    [TestCase("65536")]
    [TestCase("invalid")]
    [TestCase("")]
    public void RegisterServices_RejectsPeerPort_WhenInvalid(string port)
    {
        _settings["pmmux:--qbittorrent-peer-port"] = port;
        using var services = RegisterServices();

        Assert.Throws<ArgumentException>(() => services.GetRequiredService<QBittorrentConfig>());
    }

    [TestCase("0")]
    [TestCase("3601")]
    public void RegisterServices_RejectsInterval_WhenOutsideSupportedRange(string seconds)
    {
        _settings["pmmux:--qbittorrent-sync-seconds"] = seconds;
        using var services = RegisterServices();

        Assert.Throws<ArgumentException>(() => services.GetRequiredService<QBittorrentConfig>());
    }

    [TestCase("test-user", null)]
    [TestCase(null, "PMMUX_TEST_QBIT_PASSWORD")]
    public void RegisterServices_RejectsCredentials_WhenOnlyOneSettingIsProvided(string? username, string? variable)
    {
        _settings["pmmux:--qbittorrent-username"] = username;
        _settings["pmmux:--qbittorrent-password-env"] = variable;
        using var services = RegisterServices();

        Assert.Throws<ArgumentException>(() => services.GetRequiredService<QBittorrentConfig>());
    }

    [Test]
    public void RegisterServices_DoesNotRegisterConfiguration_WhenDisabled()
    {
        _settings["pmmux:--qbittorrent-enable"] = "false";
        using var services = RegisterServices();

        var config = services.GetService<QBittorrentConfig>();

        Assert.That(config, Is.Null);
    }

    private ServiceProvider RegisterServices()
    {
        var context = new HostBuilderContext(new Dictionary<object, object>())
        {
            Configuration = new ConfigurationBuilder().AddInMemoryCollection(_settings).Build()
        };
        var services = new ServiceCollection();

        _subject.RegisterServices(services, context);

        return services.BuildServiceProvider();
    }
}

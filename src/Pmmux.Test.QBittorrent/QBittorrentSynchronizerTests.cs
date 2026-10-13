using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;

using Mono.Nat;

using Moq;

using NUnit.Framework;

using Pmmux.Abstractions;
using Pmmux.Extensions.QBittorrent;

using QBittorrent.Client;

namespace Pmmux.Test.QBittorrent;

[TestFixture]
public class QBittorrentSynchronizerTests
{
    private Mock<IPortWarden> _warden = null!;
    private Mock<IQBittorrentClient> _client = null!;
    private Mock<IEventNotifier> _events = null!;
    private FakeTimeProvider _time = null!;
    private QBittorrentSynchronizer _subject = null!;
    private PortMapInfo[] _currentMaps = [];

    [SetUp]
    public void SetUp()
    {
        _time = new FakeTimeProvider();
        _warden = new Mock<IPortWarden>(MockBehavior.Strict);
        _currentMaps = [];
        _warden.Setup(w => w.GetPortMaps()).Returns(() => _currentMaps);
        ConfigureMappingCreation(null, 36140);
        _warden.Setup(w => w.RemovePortMapAsync(
                It.IsAny<Protocol>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Callback((Protocol protocol, int local, int external, CancellationToken _) =>
                _currentMaps = [.. _currentMaps.Where(m =>
                    m.NetworkProtocol != protocol || m.LocalPort != local || m.PublicEndpoint.Port != external)])
            .ReturnsAsync(true);
        _client = new Mock<IQBittorrentClient>(MockBehavior.Strict);
        _events = new Mock<IEventNotifier>();
        _subject = new QBittorrentSynchronizer(
            new QBittorrentConfig(),
            _warden.Object,
            _events.Object,
            _client.Object,
            Mock.Of<IMetricReporter>(),
            NullLogger<QBittorrentSynchronizer>.Instance,
            _time);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _subject.StopAsync(CancellationToken.None);
        _subject.Dispose();
    }

    [Test]
    public async Task ReconcileAsync_UpdatesPortAndVerifiesSettings_WhenApplicationDiffers()
    {
        _client.SetupSequence(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 12345, RandomPort = true, UpnpEnabled = true })
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });
        _client.Setup(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 36140 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _subject.ReconcileAsync(CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
            Assert.That(_subject.Status.MappedPort, Is.EqualTo(36140));
            Assert.That(_subject.Status.ApplicationPort, Is.EqualTo(36140));
            Assert.That(_subject.Status.LastVerifiedAt, Is.EqualTo(_time.GetUtcNow()));
        }
        _client.Verify(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        _client.Verify(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 36140 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.GetPortMaps(), Times.AtLeastOnce);
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Tcp, null, null, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Once);
        _warden.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ReconcileAsync_LogsIn_WhenCredentialsAreConfigured()
    {
        var variable = "PMMUX_TEST_QBIT_" + Guid.NewGuid().ToString("N");
        Environment.SetEnvironmentVariable(variable, "test-password");

        try
        {
            _subject.Dispose();
            _subject = new QBittorrentSynchronizer(
                new QBittorrentConfig { Username = "test-user", PasswordEnvironment = variable },
                _warden.Object,
                _events.Object,
                _client.Object,
                Mock.Of<IMetricReporter>(),
                NullLogger<QBittorrentSynchronizer>.Instance,
                _time);
            var sequence = new MockSequence();
            _client.InSequence(sequence)
                .Setup(c => c.LoginAsync("test-user", "test-password", It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _client.InSequence(sequence).Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });

            await _subject.ReconcileAsync(CancellationToken.None);

            Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
            _client.Verify(c => c.LoginAsync("test-user", "test-password", It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            Environment.SetEnvironmentVariable(variable, null);
        }
    }

    [TestCase(null, false, false)]
    [TestCase(36140, null, false)]
    [TestCase(36140, false, null)]
    public async Task ReconcileAsync_DoesNotReportSuccess_WhenSettingsRemainIncomplete(
        int? port, bool? random, bool? upnp)
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = port, RandomPort = random, UpnpEnabled = upnp });
        _client.Setup(c => c.SetPreferencesAsync(It.IsAny<Preferences>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("verification-failed"));
        Assert.That(_subject.Status.LastVerifiedAt, Is.Null);
    }

    [Test]
    public async Task ReconcileAsync_DoesNotWrite_WhenSettingsAlreadyMatch()
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
        _client.Verify(c => c.SetPreferencesAsync(It.IsAny<Preferences>(), It.IsAny<CancellationToken>()), Times.Never);
        _client.Verify(c => c.LoginAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ReconcileAsync_DoesNotWrite_WhenSameMappingIsReportedAgain()
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });
        await _subject.ReconcileAsync(CancellationToken.None);
        _warden.Setup(w => w.GetPortMaps()).Returns(Mappings(36140));

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
        _client.Verify(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        _client.Verify(c => c.SetPreferencesAsync(It.IsAny<Preferences>(), It.IsAny<CancellationToken>()), Times.Never);
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Tcp, null, null, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ReconcileAsync_RequestsConfiguredPort_WhenPortIsFixed()
    {
        _subject.Dispose();
        _subject = new QBittorrentSynchronizer(
            new QBittorrentConfig { PeerPort = 42000 },
            _warden.Object,
            _events.Object,
            _client.Object,
            Mock.Of<IMetricReporter>(),
            NullLogger<QBittorrentSynchronizer>.Instance,
            _time);
        ConfigureMappingCreation(42000, 42000);
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 42000, RandomPort = false, UpnpEnabled = false });

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Tcp, 42000, 42000, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Udp, 42000, 42000, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ReconcileAsync_DoesNotAdoptOrDeleteExistingMapping_WhenPortConflicts()
    {
        _currentMaps = Mappings(36140);
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Tcp, null, null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("port map conflict"));

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("mapping-error"));
        _client.VerifyNoOtherCalls();
        _warden.Verify(w => w.RemovePortMapAsync(
            It.IsAny<Protocol>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ReconcileAsync_RejectsDifferentPort_WhenFixedPortWasRequested()
    {
        _subject.Dispose();
        _subject = new QBittorrentSynchronizer(
            new QBittorrentConfig { PeerPort = 42000 },
            _warden.Object,
            _events.Object,
            _client.Object,
            Mock.Of<IMetricReporter>(),
            NullLogger<QBittorrentSynchronizer>.Instance,
            _time);
        ConfigureMappingCreation(42000, 42001);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("mapping-mismatch"));
        Assert.That(_currentMaps, Is.Empty);
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Tcp, 42001, 42001, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.AddPortMapAsync(
            Protocol.Udp, It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()), Times.Never);
        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ReconcileAsync_Waits_WhenMappingCannotBeCreated()
    {
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Tcp, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PortMapInfo?)null);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("waiting-for-mapping"));
        _warden.Verify(w => w.AddPortMapAsync(
            Protocol.Udp, It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()), Times.Never);
        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ReconcileAsync_RetriesMapping_WhenPreviousCreationFailed()
    {
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        await _subject.ReconcileAsync(CancellationToken.None);
        ConfigureMappingCreation(null, 36140);
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
        Assert.That(_currentMaps, Is.EquivalentTo(Mappings(36140)));
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Tcp, null, null, It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Test]
    public async Task ReconcileAsync_RemovesPartialMapping_WhenUdpCreationFails()
    {
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("mapping-error"));
        Assert.That(_currentMaps, Is.Empty);
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Tcp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Once);
        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ReconcileAsync_DoesNotCreateAnotherMapping_WhenPartialCleanupFails()
    {
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        _warden.Setup(w => w.RemovePortMapAsync(Protocol.Tcp, 36140, 36140, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        await _subject.ReconcileAsync(CancellationToken.None);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("mapping-error"));
        _warden.Verify(w => w.AddPortMapAsync(Protocol.Tcp, null, null, It.IsAny<CancellationToken>()), Times.Once);
        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ReconcileAsync_RemovesBothMappings_WhenUdpPortDoesNotMatch()
    {
        var udp = Mappings(42000)[1];
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()))
            .Callback(() => _currentMaps = [.. _currentMaps, udp])
            .ReturnsAsync(udp);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("mapping-mismatch"));
        Assert.That(_currentMaps, Is.Empty);
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Tcp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Udp, 42000, 42000, It.IsAny<CancellationToken>()), Times.Once);
        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ReconcileAsync_RejectsTranslatedMapping_WhenLocalAndPublicPortsDiffer()
    {
        var tcp = Mappings(36140)[0] with { LocalPort = 12345 };
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Tcp, null, null, It.IsAny<CancellationToken>()))
            .Callback(() => _currentMaps = [tcp])
            .ReturnsAsync(tcp);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("mapping-mismatch"));
        Assert.That(_currentMaps, Is.Empty);
        _warden.Verify(w => w.AddPortMapAsync(
            Protocol.Udp, It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()), Times.Never);
        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ReconcileAsync_RecreatesOwnedPair_WhenOneMappingIsRemoved()
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });
        await _subject.ReconcileAsync(CancellationToken.None);
        _currentMaps = [.. _currentMaps.Where(m => m.NetworkProtocol == Protocol.Udp)];
        ConfigureMappingCreation(null, 42000);
        _client.SetupSequence(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false })
            .ReturnsAsync(new Preferences { ListenPort = 42000, RandomPort = false, UpnpEnabled = false });
        _client.Setup(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 42000 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
        Assert.That(_subject.Status.MappedPort, Is.EqualTo(42000));
        Assert.That(_currentMaps, Is.EquivalentTo(Mappings(42000)));
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Tcp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task StopAsync_RemovesOnlyOwnedMappings_WhenWorkerStops()
    {
        var read = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var unrelated = Mappings(443);
        _currentMaps = unrelated;
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => read.TrySetResult())
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });
        await _subject.StartAsync(CancellationToken.None);
        await read.Task.WaitAsync(TimeSpan.FromSeconds(5));

        await _subject.StopAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("stopped"));
        Assert.That(_currentMaps, Is.EquivalentTo(unrelated));
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Tcp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.RemovePortMapAsync(Protocol.Udp, 36140, 36140, It.IsAny<CancellationToken>()), Times.Once);
        _warden.Verify(w => w.RemovePortMapAsync(
            It.IsAny<Protocol>(), 443, 443, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ReconcileAsync_UsesNewPort_WhenMappingIsReplaced()
    {
        ConfigureMappingCreation(null, 42000);
        _client.SetupSequence(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false })
            .ReturnsAsync(new Preferences { ListenPort = 42000, RandomPort = false, UpnpEnabled = false });
        _client.Setup(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 42000 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _subject.ReconcileAsync(CancellationToken.None);

        _client.Verify(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 42000 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(_subject.Status.MappedPort, Is.EqualTo(42000));
    }

    [Test]
    public async Task ReconcileAsync_IgnoresOtherBindings_WhenMultipleMappingsExist()
    {
        _currentMaps = Mappings(443).Select(m => m with { Index = 1 }).ToArray();
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.MappedPort, Is.EqualTo(36140));
        _client.Verify(c => c.SetPreferencesAsync(It.IsAny<Preferences>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ReconcileAsync_DoesNotWriteObsoletePort_WhenMappingChangesDuringRead()
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => _warden.Setup(w => w.GetPortMaps()).Returns(Mappings(42000)))
            .ReturnsAsync(new Preferences { ListenPort = 12345, RandomPort = false, UpnpEnabled = false });

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("waiting-for-mapping"));
        _client.Verify(c => c.SetPreferencesAsync(It.IsAny<Preferences>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ReconcileAsync_ReportsSuccess_WhenSameMappingIsRefreshedDuringRead()
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => _warden.Setup(w => w.GetPortMaps()).Returns(Mappings(36140)))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
        _client.Verify(c => c.SetPreferencesAsync(It.IsAny<Preferences>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ReconcileAsync_DoesNotReportSuccess_WhenMappingIsRemovedDuringWrite()
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 12345, RandomPort = false, UpnpEnabled = false });
        _client.Setup(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 36140 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()))
            .Callback(() => _warden.Setup(w => w.GetPortMaps()).Returns(Array.Empty<PortMapInfo>()))
            .Returns(Task.CompletedTask);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("waiting-for-mapping"));
        Assert.That(_subject.Status.LastVerifiedAt, Is.Null);
    }

    [Test]
    public async Task ReconcileAsync_ReportsVerificationFailure_WhenApplicationIgnoresWrite()
    {
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 12345, RandomPort = false, UpnpEnabled = false });
        _client.Setup(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 36140 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("verification-failed"));
        Assert.That(_subject.Status.LastVerifiedAt, Is.Null);
    }

    [Test]
    public async Task ReconcileAsync_Recovers_WhenApplicationReturnsAfterTimeout()
    {
        _client.SetupSequence(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException())
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });
        await _subject.ReconcileAsync(CancellationToken.None);
        var failedState = _subject.Status.State;

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(failedState, Is.EqualTo("api-error"));
        Assert.That(_subject.Status.State, Is.EqualTo("synchronized"));
    }

    [Test]
    public async Task ReconcileAsync_WaitsWithoutCallingApplication_WhenWardenHasNotStarted()
    {
        _warden.Setup(w => w.GetPortMaps()).Throws<InvalidOperationException>();

        await _subject.ReconcileAsync(CancellationToken.None);

        Assert.That(_subject.Status.State, Is.EqualTo("waiting-for-mapping"));
        _client.VerifyNoOtherCalls();
    }

    [Test]
    public async Task MappingChanged_DoesNotBlock_WhenApplicationRequestIsPending()
    {
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var response = new TaskCompletionSource<Preferences>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        _client.Setup(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => entered.TrySetResult())
            .Returns(response.Task);
        await _subject.StartAsync(CancellationToken.None);
        await entered.Task.WaitAsync(TimeSpan.FromSeconds(5));

        try
        {
            for (var i = 0; i < 100; i++)
            {
                _events.Raise(e => e.PortMapChanged += null, this, new Mapping(Protocol.Tcp, 36140, 36140));
            }

            Assert.That(response.Task.IsCompleted, Is.False);
        }
        finally
        {
            response.TrySetResult(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });
        }
    }

    [Test]
    public async Task ExecuteAsync_ReconcilesDrift_WhenPeriodicIntervalElapses()
    {
        var scheduled = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var written = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var time = new Mock<TimeProvider>();
        time.Setup(t => t.GetUtcNow()).Returns(() => _time.GetUtcNow());
        time.Setup(t => t.CreateTimer(
                It.IsAny<TimerCallback>(),
                It.IsAny<object?>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
            .Returns((TimerCallback callback, object? state, TimeSpan due, TimeSpan period) =>
            {
                var timer = _time.CreateTimer(callback, state, due, period);
                scheduled.TrySetResult();
                return timer;
            });
        _subject.Dispose();
        _subject = new QBittorrentSynchronizer(
            new QBittorrentConfig(),
            _warden.Object,
            _events.Object,
            _client.Object,
            Mock.Of<IMetricReporter>(),
            NullLogger<QBittorrentSynchronizer>.Instance,
            time.Object);
        _client.SetupSequence(c => c.GetPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false })
            .ReturnsAsync(new Preferences { ListenPort = 12345, RandomPort = true, UpnpEnabled = true })
            .ReturnsAsync(new Preferences { ListenPort = 36140, RandomPort = false, UpnpEnabled = false });
        _client.Setup(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 36140 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()))
            .Callback(() => written.TrySetResult()).Returns(Task.CompletedTask);
        await _subject.StartAsync(CancellationToken.None);
        await scheduled.Task.WaitAsync(TimeSpan.FromSeconds(5));

        _time.Advance(TimeSpan.FromSeconds(30));
        await written.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await _subject.StopAsync(CancellationToken.None);

        _client.Verify(c => c.SetPreferencesAsync(
            It.Is<Preferences>(p => p.ListenPort == 36140 && p.RandomPort == false && p.UpnpEnabled == false),
            It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(_subject.Status.State, Is.EqualTo("stopped"));
    }

    private void ConfigureMappingCreation(int? requested, int assigned)
    {
        var maps = Mappings(assigned);
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Tcp, requested, requested, It.IsAny<CancellationToken>()))
            .Callback(() => _currentMaps = [.. _currentMaps, maps[0]])
            .ReturnsAsync(maps[0]);
        _warden.Setup(w => w.AddPortMapAsync(Protocol.Udp, assigned, assigned, It.IsAny<CancellationToken>()))
            .Callback(() => _currentMaps = [.. _currentMaps, maps[1]])
            .ReturnsAsync(maps[1]);
    }

    private static PortMapInfo[] Mappings(int port)
    {
        return [.. new[] { Protocol.Tcp, Protocol.Udp }.Select(protocol => new PortMapInfo(
            protocol,
            new IPEndPoint(IPAddress.Parse("203.0.113.1"), port),
            port,
            NatProtocol.Pmp))];
    }
}

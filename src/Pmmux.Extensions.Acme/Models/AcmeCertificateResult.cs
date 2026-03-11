using System;

namespace Pmmux.Extensions.Acme.Models;

internal record AcmeCertificateResult(byte[] Pfx, DateTime ExpiresAtUtc);

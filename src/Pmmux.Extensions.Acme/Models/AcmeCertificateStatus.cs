namespace Pmmux.Extensions.Acme.Models;

internal enum AcmeCertificateStatus
{
    Pending = 0,
    Validating,
    Valid,
    RenewalDue,
    Expired,
    Error
}

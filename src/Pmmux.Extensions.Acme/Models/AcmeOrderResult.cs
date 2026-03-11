using System.Collections.Generic;

using Certes.Acme;

using Pmmux.Extensions.Acme.Abstractions;

namespace Pmmux.Extensions.Acme.Models;

internal record AcmeOrderResult(IOrderContext Order, IEnumerable<AuthorizationInfo> Authorizations);

using System;

namespace Steamworks
{
	// Token: 0x02000125 RID: 293
	public enum EHTTPStatusCode
	{
		// Token: 0x040005CD RID: 1485
		k_EHTTPStatusCodeInvalid,
		// Token: 0x040005CE RID: 1486
		k_EHTTPStatusCode100Continue = 100,
		// Token: 0x040005CF RID: 1487
		k_EHTTPStatusCode101SwitchingProtocols,
		// Token: 0x040005D0 RID: 1488
		k_EHTTPStatusCode200OK = 200,
		// Token: 0x040005D1 RID: 1489
		k_EHTTPStatusCode201Created,
		// Token: 0x040005D2 RID: 1490
		k_EHTTPStatusCode202Accepted,
		// Token: 0x040005D3 RID: 1491
		k_EHTTPStatusCode203NonAuthoritative,
		// Token: 0x040005D4 RID: 1492
		k_EHTTPStatusCode204NoContent,
		// Token: 0x040005D5 RID: 1493
		k_EHTTPStatusCode205ResetContent,
		// Token: 0x040005D6 RID: 1494
		k_EHTTPStatusCode206PartialContent,
		// Token: 0x040005D7 RID: 1495
		k_EHTTPStatusCode300MultipleChoices = 300,
		// Token: 0x040005D8 RID: 1496
		k_EHTTPStatusCode301MovedPermanently,
		// Token: 0x040005D9 RID: 1497
		k_EHTTPStatusCode302Found,
		// Token: 0x040005DA RID: 1498
		k_EHTTPStatusCode303SeeOther,
		// Token: 0x040005DB RID: 1499
		k_EHTTPStatusCode304NotModified,
		// Token: 0x040005DC RID: 1500
		k_EHTTPStatusCode305UseProxy,
		// Token: 0x040005DD RID: 1501
		k_EHTTPStatusCode307TemporaryRedirect = 307,
		// Token: 0x040005DE RID: 1502
		k_EHTTPStatusCode400BadRequest = 400,
		// Token: 0x040005DF RID: 1503
		k_EHTTPStatusCode401Unauthorized,
		// Token: 0x040005E0 RID: 1504
		k_EHTTPStatusCode402PaymentRequired,
		// Token: 0x040005E1 RID: 1505
		k_EHTTPStatusCode403Forbidden,
		// Token: 0x040005E2 RID: 1506
		k_EHTTPStatusCode404NotFound,
		// Token: 0x040005E3 RID: 1507
		k_EHTTPStatusCode405MethodNotAllowed,
		// Token: 0x040005E4 RID: 1508
		k_EHTTPStatusCode406NotAcceptable,
		// Token: 0x040005E5 RID: 1509
		k_EHTTPStatusCode407ProxyAuthRequired,
		// Token: 0x040005E6 RID: 1510
		k_EHTTPStatusCode408RequestTimeout,
		// Token: 0x040005E7 RID: 1511
		k_EHTTPStatusCode409Conflict,
		// Token: 0x040005E8 RID: 1512
		k_EHTTPStatusCode410Gone,
		// Token: 0x040005E9 RID: 1513
		k_EHTTPStatusCode411LengthRequired,
		// Token: 0x040005EA RID: 1514
		k_EHTTPStatusCode412PreconditionFailed,
		// Token: 0x040005EB RID: 1515
		k_EHTTPStatusCode413RequestEntityTooLarge,
		// Token: 0x040005EC RID: 1516
		k_EHTTPStatusCode414RequestURITooLong,
		// Token: 0x040005ED RID: 1517
		k_EHTTPStatusCode415UnsupportedMediaType,
		// Token: 0x040005EE RID: 1518
		k_EHTTPStatusCode416RequestedRangeNotSatisfiable,
		// Token: 0x040005EF RID: 1519
		k_EHTTPStatusCode417ExpectationFailed,
		// Token: 0x040005F0 RID: 1520
		k_EHTTPStatusCode4xxUnknown,
		// Token: 0x040005F1 RID: 1521
		k_EHTTPStatusCode429TooManyRequests = 429,
		// Token: 0x040005F2 RID: 1522
		k_EHTTPStatusCode500InternalServerError = 500,
		// Token: 0x040005F3 RID: 1523
		k_EHTTPStatusCode501NotImplemented,
		// Token: 0x040005F4 RID: 1524
		k_EHTTPStatusCode502BadGateway,
		// Token: 0x040005F5 RID: 1525
		k_EHTTPStatusCode503ServiceUnavailable,
		// Token: 0x040005F6 RID: 1526
		k_EHTTPStatusCode504GatewayTimeout,
		// Token: 0x040005F7 RID: 1527
		k_EHTTPStatusCode505HTTPVersionNotSupported,
		// Token: 0x040005F8 RID: 1528
		k_EHTTPStatusCode5xxUnknown = 599
	}
}

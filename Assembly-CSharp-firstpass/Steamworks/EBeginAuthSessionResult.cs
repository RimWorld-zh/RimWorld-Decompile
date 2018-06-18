using System;

namespace Steamworks
{
	// Token: 0x02000116 RID: 278
	public enum EBeginAuthSessionResult
	{
		// Token: 0x04000538 RID: 1336
		k_EBeginAuthSessionResultOK,
		// Token: 0x04000539 RID: 1337
		k_EBeginAuthSessionResultInvalidTicket,
		// Token: 0x0400053A RID: 1338
		k_EBeginAuthSessionResultDuplicateRequest,
		// Token: 0x0400053B RID: 1339
		k_EBeginAuthSessionResultInvalidVersion,
		// Token: 0x0400053C RID: 1340
		k_EBeginAuthSessionResultGameMismatch,
		// Token: 0x0400053D RID: 1341
		k_EBeginAuthSessionResultExpiredTicket
	}
}

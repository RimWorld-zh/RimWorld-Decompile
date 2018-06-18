using System;

namespace Steamworks
{
	// Token: 0x02000117 RID: 279
	public enum EAuthSessionResponse
	{
		// Token: 0x0400053F RID: 1343
		k_EAuthSessionResponseOK,
		// Token: 0x04000540 RID: 1344
		k_EAuthSessionResponseUserNotConnectedToSteam,
		// Token: 0x04000541 RID: 1345
		k_EAuthSessionResponseNoLicenseOrExpired,
		// Token: 0x04000542 RID: 1346
		k_EAuthSessionResponseVACBanned,
		// Token: 0x04000543 RID: 1347
		k_EAuthSessionResponseLoggedInElseWhere,
		// Token: 0x04000544 RID: 1348
		k_EAuthSessionResponseVACCheckTimedOut,
		// Token: 0x04000545 RID: 1349
		k_EAuthSessionResponseAuthTicketCanceled,
		// Token: 0x04000546 RID: 1350
		k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
		// Token: 0x04000547 RID: 1351
		k_EAuthSessionResponseAuthTicketInvalid,
		// Token: 0x04000548 RID: 1352
		k_EAuthSessionResponsePublisherIssuedBan
	}
}

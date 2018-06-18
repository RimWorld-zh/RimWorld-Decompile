using System;

namespace Steamworks
{
	// Token: 0x0200011B RID: 283
	[Flags]
	public enum EAppOwnershipFlags
	{
		// Token: 0x04000561 RID: 1377
		k_EAppOwnershipFlags_None = 0,
		// Token: 0x04000562 RID: 1378
		k_EAppOwnershipFlags_OwnsLicense = 1,
		// Token: 0x04000563 RID: 1379
		k_EAppOwnershipFlags_FreeLicense = 2,
		// Token: 0x04000564 RID: 1380
		k_EAppOwnershipFlags_RegionRestricted = 4,
		// Token: 0x04000565 RID: 1381
		k_EAppOwnershipFlags_LowViolence = 8,
		// Token: 0x04000566 RID: 1382
		k_EAppOwnershipFlags_InvalidPlatform = 16,
		// Token: 0x04000567 RID: 1383
		k_EAppOwnershipFlags_SharedLicense = 32,
		// Token: 0x04000568 RID: 1384
		k_EAppOwnershipFlags_FreeWeekend = 64,
		// Token: 0x04000569 RID: 1385
		k_EAppOwnershipFlags_RetailLicense = 128,
		// Token: 0x0400056A RID: 1386
		k_EAppOwnershipFlags_LicenseLocked = 256,
		// Token: 0x0400056B RID: 1387
		k_EAppOwnershipFlags_LicensePending = 512,
		// Token: 0x0400056C RID: 1388
		k_EAppOwnershipFlags_LicenseExpired = 1024,
		// Token: 0x0400056D RID: 1389
		k_EAppOwnershipFlags_LicensePermanent = 2048,
		// Token: 0x0400056E RID: 1390
		k_EAppOwnershipFlags_LicenseRecurring = 4096,
		// Token: 0x0400056F RID: 1391
		k_EAppOwnershipFlags_LicenseCanceled = 8192,
		// Token: 0x04000570 RID: 1392
		k_EAppOwnershipFlags_AutoGrant = 16384
	}
}

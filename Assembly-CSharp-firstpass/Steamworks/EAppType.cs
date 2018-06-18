using System;

namespace Steamworks
{
	// Token: 0x0200011C RID: 284
	[Flags]
	public enum EAppType
	{
		// Token: 0x04000572 RID: 1394
		k_EAppType_Invalid = 0,
		// Token: 0x04000573 RID: 1395
		k_EAppType_Game = 1,
		// Token: 0x04000574 RID: 1396
		k_EAppType_Application = 2,
		// Token: 0x04000575 RID: 1397
		k_EAppType_Tool = 4,
		// Token: 0x04000576 RID: 1398
		k_EAppType_Demo = 8,
		// Token: 0x04000577 RID: 1399
		k_EAppType_Media_DEPRECATED = 16,
		// Token: 0x04000578 RID: 1400
		k_EAppType_DLC = 32,
		// Token: 0x04000579 RID: 1401
		k_EAppType_Guide = 64,
		// Token: 0x0400057A RID: 1402
		k_EAppType_Driver = 128,
		// Token: 0x0400057B RID: 1403
		k_EAppType_Config = 256,
		// Token: 0x0400057C RID: 1404
		k_EAppType_Hardware = 512,
		// Token: 0x0400057D RID: 1405
		k_EAppType_Video = 2048,
		// Token: 0x0400057E RID: 1406
		k_EAppType_Plugin = 4096,
		// Token: 0x0400057F RID: 1407
		k_EAppType_Music = 8192,
		// Token: 0x04000580 RID: 1408
		k_EAppType_Shortcut = 1073741824,
		// Token: 0x04000581 RID: 1409
		k_EAppType_DepotOnly = -2147483647
	}
}

using System;

namespace Steamworks
{
	// Token: 0x02000123 RID: 291
	public enum EBroadcastUploadResult
	{
		// Token: 0x040005B7 RID: 1463
		k_EBroadcastUploadResultNone,
		// Token: 0x040005B8 RID: 1464
		k_EBroadcastUploadResultOK,
		// Token: 0x040005B9 RID: 1465
		k_EBroadcastUploadResultInitFailed,
		// Token: 0x040005BA RID: 1466
		k_EBroadcastUploadResultFrameFailed,
		// Token: 0x040005BB RID: 1467
		k_EBroadcastUploadResultTimeout,
		// Token: 0x040005BC RID: 1468
		k_EBroadcastUploadResultBandwidthExceeded,
		// Token: 0x040005BD RID: 1469
		k_EBroadcastUploadResultLowFPS,
		// Token: 0x040005BE RID: 1470
		k_EBroadcastUploadResultMissingKeyFrames,
		// Token: 0x040005BF RID: 1471
		k_EBroadcastUploadResultNoConnection,
		// Token: 0x040005C0 RID: 1472
		k_EBroadcastUploadResultRelayFailed,
		// Token: 0x040005C1 RID: 1473
		k_EBroadcastUploadResultSettingsChanged,
		// Token: 0x040005C2 RID: 1474
		k_EBroadcastUploadResultMissingAudio,
		// Token: 0x040005C3 RID: 1475
		k_EBroadcastUploadResultTooFarBehind
	}
}

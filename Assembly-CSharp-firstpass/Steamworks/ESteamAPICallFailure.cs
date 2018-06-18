using System;

namespace Steamworks
{
	// Token: 0x0200010D RID: 269
	public enum ESteamAPICallFailure
	{
		// Token: 0x0400049D RID: 1181
		k_ESteamAPICallFailureNone = -1,
		// Token: 0x0400049E RID: 1182
		k_ESteamAPICallFailureSteamGone,
		// Token: 0x0400049F RID: 1183
		k_ESteamAPICallFailureNetworkFailure,
		// Token: 0x040004A0 RID: 1184
		k_ESteamAPICallFailureInvalidHandle,
		// Token: 0x040004A1 RID: 1185
		k_ESteamAPICallFailureMismatchedCallback
	}
}

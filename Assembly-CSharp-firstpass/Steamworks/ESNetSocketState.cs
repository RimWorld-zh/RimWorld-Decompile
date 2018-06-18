using System;

namespace Steamworks
{
	// Token: 0x020000F6 RID: 246
	public enum ESNetSocketState
	{
		// Token: 0x040003F9 RID: 1017
		k_ESNetSocketStateInvalid,
		// Token: 0x040003FA RID: 1018
		k_ESNetSocketStateConnected,
		// Token: 0x040003FB RID: 1019
		k_ESNetSocketStateInitiated = 10,
		// Token: 0x040003FC RID: 1020
		k_ESNetSocketStateLocalCandidatesFound,
		// Token: 0x040003FD RID: 1021
		k_ESNetSocketStateReceivedRemoteCandidates,
		// Token: 0x040003FE RID: 1022
		k_ESNetSocketStateChallengeHandshake = 15,
		// Token: 0x040003FF RID: 1023
		k_ESNetSocketStateDisconnecting = 21,
		// Token: 0x04000400 RID: 1024
		k_ESNetSocketStateLocalDisconnect,
		// Token: 0x04000401 RID: 1025
		k_ESNetSocketStateTimeoutDuringConnect,
		// Token: 0x04000402 RID: 1026
		k_ESNetSocketStateRemoteEndDisconnected,
		// Token: 0x04000403 RID: 1027
		k_ESNetSocketStateConnectionBroken
	}
}

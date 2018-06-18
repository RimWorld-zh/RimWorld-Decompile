using System;

namespace Steamworks
{
	// Token: 0x020000E9 RID: 233
	[Flags]
	public enum EPersonaChange
	{
		// Token: 0x04000384 RID: 900
		k_EPersonaChangeName = 1,
		// Token: 0x04000385 RID: 901
		k_EPersonaChangeStatus = 2,
		// Token: 0x04000386 RID: 902
		k_EPersonaChangeComeOnline = 4,
		// Token: 0x04000387 RID: 903
		k_EPersonaChangeGoneOffline = 8,
		// Token: 0x04000388 RID: 904
		k_EPersonaChangeGamePlayed = 16,
		// Token: 0x04000389 RID: 905
		k_EPersonaChangeGameServer = 32,
		// Token: 0x0400038A RID: 906
		k_EPersonaChangeAvatar = 64,
		// Token: 0x0400038B RID: 907
		k_EPersonaChangeJoinedSource = 128,
		// Token: 0x0400038C RID: 908
		k_EPersonaChangeLeftSource = 256,
		// Token: 0x0400038D RID: 909
		k_EPersonaChangeRelationshipChanged = 512,
		// Token: 0x0400038E RID: 910
		k_EPersonaChangeNameFirstSet = 1024,
		// Token: 0x0400038F RID: 911
		k_EPersonaChangeFacebookInfo = 2048,
		// Token: 0x04000390 RID: 912
		k_EPersonaChangeNickname = 4096,
		// Token: 0x04000391 RID: 913
		k_EPersonaChangeSteamLevel = 8192
	}
}

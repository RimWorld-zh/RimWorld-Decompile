using System;

namespace Steamworks
{
	// Token: 0x020000E6 RID: 230
	[Flags]
	public enum EFriendFlags
	{
		// Token: 0x0400036A RID: 874
		k_EFriendFlagNone = 0,
		// Token: 0x0400036B RID: 875
		k_EFriendFlagBlocked = 1,
		// Token: 0x0400036C RID: 876
		k_EFriendFlagFriendshipRequested = 2,
		// Token: 0x0400036D RID: 877
		k_EFriendFlagImmediate = 4,
		// Token: 0x0400036E RID: 878
		k_EFriendFlagClanMember = 8,
		// Token: 0x0400036F RID: 879
		k_EFriendFlagOnGameServer = 16,
		// Token: 0x04000370 RID: 880
		k_EFriendFlagRequestingFriendship = 128,
		// Token: 0x04000371 RID: 881
		k_EFriendFlagRequestingInfo = 256,
		// Token: 0x04000372 RID: 882
		k_EFriendFlagIgnored = 512,
		// Token: 0x04000373 RID: 883
		k_EFriendFlagIgnoredFriend = 1024,
		// Token: 0x04000374 RID: 884
		k_EFriendFlagSuggested = 2048,
		// Token: 0x04000375 RID: 885
		k_EFriendFlagAll = 65535
	}
}

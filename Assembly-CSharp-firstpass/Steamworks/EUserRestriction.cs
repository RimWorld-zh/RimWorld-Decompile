using System;

namespace Steamworks
{
	// Token: 0x020000E7 RID: 231
	public enum EUserRestriction
	{
		// Token: 0x04000377 RID: 887
		k_nUserRestrictionNone,
		// Token: 0x04000378 RID: 888
		k_nUserRestrictionUnknown,
		// Token: 0x04000379 RID: 889
		k_nUserRestrictionAnyChat,
		// Token: 0x0400037A RID: 890
		k_nUserRestrictionVoiceChat = 4,
		// Token: 0x0400037B RID: 891
		k_nUserRestrictionGroupChat = 8,
		// Token: 0x0400037C RID: 892
		k_nUserRestrictionRating = 16,
		// Token: 0x0400037D RID: 893
		k_nUserRestrictionGameInvites = 32,
		// Token: 0x0400037E RID: 894
		k_nUserRestrictionTrading = 64
	}
}

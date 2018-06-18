using System;

namespace Steamworks
{
	// Token: 0x020000F2 RID: 242
	[Flags]
	public enum EChatMemberStateChange
	{
		// Token: 0x040003E2 RID: 994
		k_EChatMemberStateChangeEntered = 1,
		// Token: 0x040003E3 RID: 995
		k_EChatMemberStateChangeLeft = 2,
		// Token: 0x040003E4 RID: 996
		k_EChatMemberStateChangeDisconnected = 4,
		// Token: 0x040003E5 RID: 997
		k_EChatMemberStateChangeKicked = 8,
		// Token: 0x040003E6 RID: 998
		k_EChatMemberStateChangeBanned = 16
	}
}

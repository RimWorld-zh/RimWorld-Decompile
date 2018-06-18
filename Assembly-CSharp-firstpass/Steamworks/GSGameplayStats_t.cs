using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000057 RID: 87
	[CallbackIdentity(207)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSGameplayStats_t
	{
		// Token: 0x040000AB RID: 171
		public const int k_iCallback = 207;

		// Token: 0x040000AC RID: 172
		public EResult m_eResult;

		// Token: 0x040000AD RID: 173
		public int m_nRank;

		// Token: 0x040000AE RID: 174
		public uint m_unTotalConnects;

		// Token: 0x040000AF RID: 175
		public uint m_unTotalMinutesPlayed;
	}
}

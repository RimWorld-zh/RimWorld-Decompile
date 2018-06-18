using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000059 RID: 89
	[CallbackIdentity(209)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSReputation_t
	{
		// Token: 0x040000B5 RID: 181
		public const int k_iCallback = 209;

		// Token: 0x040000B6 RID: 182
		public EResult m_eResult;

		// Token: 0x040000B7 RID: 183
		public uint m_unReputationScore;

		// Token: 0x040000B8 RID: 184
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x040000B9 RID: 185
		public uint m_unBannedIP;

		// Token: 0x040000BA RID: 186
		public ushort m_usBannedPort;

		// Token: 0x040000BB RID: 187
		public ulong m_ulBannedGameID;

		// Token: 0x040000BC RID: 188
		public uint m_unBanExpires;
	}
}

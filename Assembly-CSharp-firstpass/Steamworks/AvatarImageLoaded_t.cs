using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000042 RID: 66
	[CallbackIdentity(334)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AvatarImageLoaded_t
	{
		// Token: 0x04000064 RID: 100
		public const int k_iCallback = 334;

		// Token: 0x04000065 RID: 101
		public CSteamID m_steamID;

		// Token: 0x04000066 RID: 102
		public int m_iImage;

		// Token: 0x04000067 RID: 103
		public int m_iWide;

		// Token: 0x04000068 RID: 104
		public int m_iTall;
	}
}

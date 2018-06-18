using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000058 RID: 88
	[CallbackIdentity(208)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GSClientGroupStatus_t
	{
		// Token: 0x040000B0 RID: 176
		public const int k_iCallback = 208;

		// Token: 0x040000B1 RID: 177
		public CSteamID m_SteamIDUser;

		// Token: 0x040000B2 RID: 178
		public CSteamID m_SteamIDGroup;

		// Token: 0x040000B3 RID: 179
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bMember;

		// Token: 0x040000B4 RID: 180
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bOfficer;
	}
}

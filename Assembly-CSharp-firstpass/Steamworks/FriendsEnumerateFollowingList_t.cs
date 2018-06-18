using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004E RID: 78
	[CallbackIdentity(346)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsEnumerateFollowingList_t
	{
		// Token: 0x0400008F RID: 143
		public const int k_iCallback = 346;

		// Token: 0x04000090 RID: 144
		public EResult m_eResult;

		// Token: 0x04000091 RID: 145
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public CSteamID[] m_rgSteamID;

		// Token: 0x04000092 RID: 146
		public int m_nResultsReturned;

		// Token: 0x04000093 RID: 147
		public int m_nTotalResultCount;
	}
}

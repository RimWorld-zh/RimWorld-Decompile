using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CC RID: 204
	[CallbackIdentity(1101)]
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	public struct UserStatsReceived_t
	{
		// Token: 0x0400026A RID: 618
		public const int k_iCallback = 1101;

		// Token: 0x0400026B RID: 619
		[FieldOffset(0)]
		public ulong m_nGameID;

		// Token: 0x0400026C RID: 620
		[FieldOffset(8)]
		public EResult m_eResult;

		// Token: 0x0400026D RID: 621
		[FieldOffset(12)]
		public CSteamID m_steamIDUser;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000082 RID: 130
	[CallbackIdentity(510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyMatchList_t
	{
		// Token: 0x0400016A RID: 362
		public const int k_iCallback = 510;

		// Token: 0x0400016B RID: 363
		public uint m_nLobbiesMatching;
	}
}

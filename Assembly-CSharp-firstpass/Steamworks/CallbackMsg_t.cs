using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012E RID: 302
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CallbackMsg_t
	{
		// Token: 0x04000635 RID: 1589
		public int m_hSteamUser;

		// Token: 0x04000636 RID: 1590
		public int m_iCallback;

		// Token: 0x04000637 RID: 1591
		public IntPtr m_pubParam;

		// Token: 0x04000638 RID: 1592
		public int m_cubParam;
	}
}

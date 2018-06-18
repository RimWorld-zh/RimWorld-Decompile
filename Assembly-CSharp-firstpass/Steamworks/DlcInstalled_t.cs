using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003A RID: 58
	[CallbackIdentity(1005)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DlcInstalled_t
	{
		// Token: 0x0400004F RID: 79
		public const int k_iCallback = 1005;

		// Token: 0x04000050 RID: 80
		public AppId_t m_nAppID;
	}
}

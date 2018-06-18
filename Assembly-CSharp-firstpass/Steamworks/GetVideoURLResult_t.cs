using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E0 RID: 224
	[CallbackIdentity(4611)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetVideoURLResult_t
	{
		// Token: 0x040002A6 RID: 678
		public const int k_iCallback = 4611;

		// Token: 0x040002A7 RID: 679
		public EResult m_eResult;

		// Token: 0x040002A8 RID: 680
		public AppId_t m_unVideoAppID;

		// Token: 0x040002A9 RID: 681
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;
	}
}

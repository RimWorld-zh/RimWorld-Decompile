using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000040 RID: 64
	[CallbackIdentity(332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameServerChangeRequested_t
	{
		// Token: 0x0400005E RID: 94
		public const int k_iCallback = 332;

		// Token: 0x0400005F RID: 95
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string m_rgchServer;

		// Token: 0x04000060 RID: 96
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string m_rgchPassword;
	}
}

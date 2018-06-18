using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000053 RID: 83
	[CallbackIdentity(202)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientDeny_t
	{
		// Token: 0x0400009E RID: 158
		public const int k_iCallback = 202;

		// Token: 0x0400009F RID: 159
		public CSteamID m_SteamID;

		// Token: 0x040000A0 RID: 160
		public EDenyReason m_eDenyReason;

		// Token: 0x040000A1 RID: 161
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchOptionalText;
	}
}

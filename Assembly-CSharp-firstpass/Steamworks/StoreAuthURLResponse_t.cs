using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CB RID: 203
	[CallbackIdentity(165)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StoreAuthURLResponse_t
	{
		// Token: 0x04000268 RID: 616
		public const int k_iCallback = 165;

		// Token: 0x04000269 RID: 617
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string m_szURL;
	}
}

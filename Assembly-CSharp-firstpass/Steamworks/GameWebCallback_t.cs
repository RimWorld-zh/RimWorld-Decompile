using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CA RID: 202
	[CallbackIdentity(164)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameWebCallback_t
	{
		// Token: 0x04000266 RID: 614
		public const int k_iCallback = 164;

		// Token: 0x04000267 RID: 615
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szURL;
	}
}

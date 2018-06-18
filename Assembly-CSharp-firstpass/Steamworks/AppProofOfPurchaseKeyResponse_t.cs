using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003C RID: 60
	[CallbackIdentity(1013)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AppProofOfPurchaseKeyResponse_t
	{
		// Token: 0x04000054 RID: 84
		public const int k_iCallback = 1013;

		// Token: 0x04000055 RID: 85
		public EResult m_eResult;

		// Token: 0x04000056 RID: 86
		public uint m_nAppID;

		// Token: 0x04000057 RID: 87
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string m_rgchKey;
	}
}

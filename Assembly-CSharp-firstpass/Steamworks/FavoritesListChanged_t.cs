using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200007B RID: 123
	[CallbackIdentity(502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListChanged_t
	{
		// Token: 0x04000146 RID: 326
		public const int k_iCallback = 502;

		// Token: 0x04000147 RID: 327
		public uint m_nIP;

		// Token: 0x04000148 RID: 328
		public uint m_nQueryPort;

		// Token: 0x04000149 RID: 329
		public uint m_nConnPort;

		// Token: 0x0400014A RID: 330
		public uint m_nAppID;

		// Token: 0x0400014B RID: 331
		public uint m_nFlags;

		// Token: 0x0400014C RID: 332
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAdd;

		// Token: 0x0400014D RID: 333
		public AccountID_t m_unAccountId;
	}
}

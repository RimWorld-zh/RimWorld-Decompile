using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200005A RID: 90
	[CallbackIdentity(210)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AssociateWithClanResult_t
	{
		// Token: 0x040000BD RID: 189
		public const int k_iCallback = 210;

		// Token: 0x040000BE RID: 190
		public EResult m_eResult;
	}
}

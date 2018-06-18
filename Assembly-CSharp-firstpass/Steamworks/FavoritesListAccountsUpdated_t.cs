using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000085 RID: 133
	[CallbackIdentity(516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListAccountsUpdated_t
	{
		// Token: 0x04000173 RID: 371
		public const int k_iCallback = 516;

		// Token: 0x04000174 RID: 372
		public EResult m_eResult;
	}
}

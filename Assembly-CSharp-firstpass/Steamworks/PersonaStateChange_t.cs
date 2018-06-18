using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003E RID: 62
	[CallbackIdentity(304)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct PersonaStateChange_t
	{
		// Token: 0x04000059 RID: 89
		public const int k_iCallback = 304;

		// Token: 0x0400005A RID: 90
		public ulong m_ulSteamID;

		// Token: 0x0400005B RID: 91
		public EPersonaChange m_nChangeFlags;
	}
}

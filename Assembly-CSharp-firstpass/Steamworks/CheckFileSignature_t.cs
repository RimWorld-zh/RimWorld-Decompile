using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DC RID: 220
	[CallbackIdentity(705)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CheckFileSignature_t
	{
		// Token: 0x0400029E RID: 670
		public const int k_iCallback = 705;

		// Token: 0x0400029F RID: 671
		public ECheckFileSignature m_eCheckFileSignature;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012C RID: 300
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamParamStringArray_t
	{
		// Token: 0x04000619 RID: 1561
		public IntPtr m_ppStrings;

		// Token: 0x0400061A RID: 1562
		public int m_nNumStrings;
	}
}

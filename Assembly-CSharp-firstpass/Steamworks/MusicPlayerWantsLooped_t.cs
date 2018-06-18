using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000091 RID: 145
	[CallbackIdentity(4110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsLooped_t
	{
		// Token: 0x04000182 RID: 386
		public const int k_iCallback = 4110;

		// Token: 0x04000183 RID: 387
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLooped;
	}
}

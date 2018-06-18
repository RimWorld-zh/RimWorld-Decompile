using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000090 RID: 144
	[CallbackIdentity(4109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsShuffled_t
	{
		// Token: 0x04000180 RID: 384
		public const int k_iCallback = 4109;

		// Token: 0x04000181 RID: 385
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bShuffled;
	}
}

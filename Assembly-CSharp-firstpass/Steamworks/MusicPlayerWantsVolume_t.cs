using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000092 RID: 146
	[CallbackIdentity(4011)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsVolume_t
	{
		// Token: 0x04000184 RID: 388
		public const int k_iCallback = 4011;

		// Token: 0x04000185 RID: 389
		public float m_flNewVolume;
	}
}

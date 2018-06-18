using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000087 RID: 135
	[CallbackIdentity(4002)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct VolumeHasChanged_t
	{
		// Token: 0x04000176 RID: 374
		public const int k_iCallback = 4002;

		// Token: 0x04000177 RID: 375
		public float m_flNewVolume;
	}
}

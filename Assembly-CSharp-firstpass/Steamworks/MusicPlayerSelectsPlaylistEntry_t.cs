using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000094 RID: 148
	[CallbackIdentity(4013)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsPlaylistEntry_t
	{
		// Token: 0x04000188 RID: 392
		public const int k_iCallback = 4013;

		// Token: 0x04000189 RID: 393
		public int nID;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000093 RID: 147
	[CallbackIdentity(4012)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsQueueEntry_t
	{
		// Token: 0x04000186 RID: 390
		public const int k_iCallback = 4012;

		// Token: 0x04000187 RID: 391
		public int nID;
	}
}

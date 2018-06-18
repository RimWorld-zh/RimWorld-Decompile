using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000095 RID: 149
	[CallbackIdentity(4114)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsPlayingRepeatStatus_t
	{
		// Token: 0x0400018A RID: 394
		public const int k_iCallback = 4114;

		// Token: 0x0400018B RID: 395
		public int m_nPlayingRepeatStatus;
	}
}

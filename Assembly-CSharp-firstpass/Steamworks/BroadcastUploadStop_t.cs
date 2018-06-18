using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DF RID: 223
	[CallbackIdentity(4605)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStop_t
	{
		// Token: 0x040002A4 RID: 676
		public const int k_iCallback = 4605;

		// Token: 0x040002A5 RID: 677
		public EBroadcastUploadResult m_eResult;
	}
}

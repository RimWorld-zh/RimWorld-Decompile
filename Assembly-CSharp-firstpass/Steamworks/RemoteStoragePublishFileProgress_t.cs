using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B2 RID: 178
	[CallbackIdentity(1329)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileProgress_t
	{
		// Token: 0x04000216 RID: 534
		public const int k_iCallback = 1329;

		// Token: 0x04000217 RID: 535
		public double m_dPercentFile;

		// Token: 0x04000218 RID: 536
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPreview;
	}
}

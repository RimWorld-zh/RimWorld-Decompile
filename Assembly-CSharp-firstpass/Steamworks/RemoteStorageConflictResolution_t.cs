using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009D RID: 157
	[CallbackIdentity(1306)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageConflictResolution_t
	{
		// Token: 0x040001A7 RID: 423
		public const int k_iCallback = 1306;

		// Token: 0x040001A8 RID: 424
		public AppId_t m_nAppID;

		// Token: 0x040001A9 RID: 425
		public EResult m_eResult;
	}
}

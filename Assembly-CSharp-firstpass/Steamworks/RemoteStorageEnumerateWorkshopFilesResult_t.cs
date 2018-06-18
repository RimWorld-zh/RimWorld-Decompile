using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A8 RID: 168
	[CallbackIdentity(1319)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateWorkshopFilesResult_t
	{
		// Token: 0x040001E7 RID: 487
		public const int k_iCallback = 1319;

		// Token: 0x040001E8 RID: 488
		public EResult m_eResult;

		// Token: 0x040001E9 RID: 489
		public int m_nResultsReturned;

		// Token: 0x040001EA RID: 490
		public int m_nTotalResultCount;

		// Token: 0x040001EB RID: 491
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x040001EC RID: 492
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public float[] m_rgScore;

		// Token: 0x040001ED RID: 493
		public AppId_t m_nAppId;

		// Token: 0x040001EE RID: 494
		public uint m_unStartIndex;
	}
}

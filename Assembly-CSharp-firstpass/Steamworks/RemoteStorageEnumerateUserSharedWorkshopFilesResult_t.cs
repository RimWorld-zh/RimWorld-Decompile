using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AF RID: 175
	[CallbackIdentity(1326)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSharedWorkshopFilesResult_t
	{
		// Token: 0x04000206 RID: 518
		public const int k_iCallback = 1326;

		// Token: 0x04000207 RID: 519
		public EResult m_eResult;

		// Token: 0x04000208 RID: 520
		public int m_nResultsReturned;

		// Token: 0x04000209 RID: 521
		public int m_nTotalResultCount;

		// Token: 0x0400020A RID: 522
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}

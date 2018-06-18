using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A7 RID: 167
	[CallbackIdentity(1318)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedFileDetailsResult_t
	{
		// Token: 0x040001D1 RID: 465
		public const int k_iCallback = 1318;

		// Token: 0x040001D2 RID: 466
		public EResult m_eResult;

		// Token: 0x040001D3 RID: 467
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001D4 RID: 468
		public AppId_t m_nCreatorAppID;

		// Token: 0x040001D5 RID: 469
		public AppId_t m_nConsumerAppID;

		// Token: 0x040001D6 RID: 470
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		public string m_rgchTitle;

		// Token: 0x040001D7 RID: 471
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
		public string m_rgchDescription;

		// Token: 0x040001D8 RID: 472
		public UGCHandle_t m_hFile;

		// Token: 0x040001D9 RID: 473
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x040001DA RID: 474
		public ulong m_ulSteamIDOwner;

		// Token: 0x040001DB RID: 475
		public uint m_rtimeCreated;

		// Token: 0x040001DC RID: 476
		public uint m_rtimeUpdated;

		// Token: 0x040001DD RID: 477
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x040001DE RID: 478
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x040001DF RID: 479
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string m_rgchTags;

		// Token: 0x040001E0 RID: 480
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x040001E1 RID: 481
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x040001E2 RID: 482
		public int m_nFileSize;

		// Token: 0x040001E3 RID: 483
		public int m_nPreviewFileSize;

		// Token: 0x040001E4 RID: 484
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;

		// Token: 0x040001E5 RID: 485
		public EWorkshopFileType m_eFileType;

		// Token: 0x040001E6 RID: 486
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;
	}
}

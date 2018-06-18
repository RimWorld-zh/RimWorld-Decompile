using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012D RID: 301
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCDetails_t
	{
		// Token: 0x0400061B RID: 1563
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400061C RID: 1564
		public EResult m_eResult;

		// Token: 0x0400061D RID: 1565
		public EWorkshopFileType m_eFileType;

		// Token: 0x0400061E RID: 1566
		public AppId_t m_nCreatorAppID;

		// Token: 0x0400061F RID: 1567
		public AppId_t m_nConsumerAppID;

		// Token: 0x04000620 RID: 1568
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		public string m_rgchTitle;

		// Token: 0x04000621 RID: 1569
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
		public string m_rgchDescription;

		// Token: 0x04000622 RID: 1570
		public ulong m_ulSteamIDOwner;

		// Token: 0x04000623 RID: 1571
		public uint m_rtimeCreated;

		// Token: 0x04000624 RID: 1572
		public uint m_rtimeUpdated;

		// Token: 0x04000625 RID: 1573
		public uint m_rtimeAddedToUserList;

		// Token: 0x04000626 RID: 1574
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000627 RID: 1575
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000628 RID: 1576
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;

		// Token: 0x04000629 RID: 1577
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x0400062A RID: 1578
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string m_rgchTags;

		// Token: 0x0400062B RID: 1579
		public UGCHandle_t m_hFile;

		// Token: 0x0400062C RID: 1580
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x0400062D RID: 1581
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x0400062E RID: 1582
		public int m_nFileSize;

		// Token: 0x0400062F RID: 1583
		public int m_nPreviewFileSize;

		// Token: 0x04000630 RID: 1584
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;

		// Token: 0x04000631 RID: 1585
		public uint m_unVotesUp;

		// Token: 0x04000632 RID: 1586
		public uint m_unVotesDown;

		// Token: 0x04000633 RID: 1587
		public float m_flScore;

		// Token: 0x04000634 RID: 1588
		public uint m_unNumChildren;
	}
}

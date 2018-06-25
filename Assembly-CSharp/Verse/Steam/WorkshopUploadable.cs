using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC8 RID: 4040
	public interface WorkshopUploadable
	{
		// Token: 0x060061A4 RID: 24996
		bool CanToUploadToWorkshop();

		// Token: 0x060061A5 RID: 24997
		void PrepareForWorkshopUpload();

		// Token: 0x060061A6 RID: 24998
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x060061A7 RID: 24999
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x060061A8 RID: 25000
		string GetWorkshopName();

		// Token: 0x060061A9 RID: 25001
		string GetWorkshopDescription();

		// Token: 0x060061AA RID: 25002
		string GetWorkshopPreviewImagePath();

		// Token: 0x060061AB RID: 25003
		IList<string> GetWorkshopTags();

		// Token: 0x060061AC RID: 25004
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x060061AD RID: 25005
		WorkshopItemHook GetWorkshopItemHook();
	}
}

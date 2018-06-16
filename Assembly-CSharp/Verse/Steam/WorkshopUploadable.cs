using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC4 RID: 4036
	public interface WorkshopUploadable
	{
		// Token: 0x0600616D RID: 24941
		bool CanToUploadToWorkshop();

		// Token: 0x0600616E RID: 24942
		void PrepareForWorkshopUpload();

		// Token: 0x0600616F RID: 24943
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x06006170 RID: 24944
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x06006171 RID: 24945
		string GetWorkshopName();

		// Token: 0x06006172 RID: 24946
		string GetWorkshopDescription();

		// Token: 0x06006173 RID: 24947
		string GetWorkshopPreviewImagePath();

		// Token: 0x06006174 RID: 24948
		IList<string> GetWorkshopTags();

		// Token: 0x06006175 RID: 24949
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x06006176 RID: 24950
		WorkshopItemHook GetWorkshopItemHook();
	}
}

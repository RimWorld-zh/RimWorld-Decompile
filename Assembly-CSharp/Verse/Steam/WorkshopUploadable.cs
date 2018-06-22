using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC4 RID: 4036
	public interface WorkshopUploadable
	{
		// Token: 0x06006194 RID: 24980
		bool CanToUploadToWorkshop();

		// Token: 0x06006195 RID: 24981
		void PrepareForWorkshopUpload();

		// Token: 0x06006196 RID: 24982
		PublishedFileId_t GetPublishedFileId();

		// Token: 0x06006197 RID: 24983
		void SetPublishedFileId(PublishedFileId_t pfid);

		// Token: 0x06006198 RID: 24984
		string GetWorkshopName();

		// Token: 0x06006199 RID: 24985
		string GetWorkshopDescription();

		// Token: 0x0600619A RID: 24986
		string GetWorkshopPreviewImagePath();

		// Token: 0x0600619B RID: 24987
		IList<string> GetWorkshopTags();

		// Token: 0x0600619C RID: 24988
		DirectoryInfo GetWorkshopUploadDirectory();

		// Token: 0x0600619D RID: 24989
		WorkshopItemHook GetWorkshopItemHook();
	}
}

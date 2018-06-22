using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC5 RID: 4037
	public class WorkshopItemHook
	{
		// Token: 0x0600619E RID: 24990 RVA: 0x003142E0 File Offset: 0x003126E0
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x0600619F RID: 24991 RVA: 0x00314318 File Offset: 0x00312718
		// (set) Token: 0x060061A0 RID: 24992 RVA: 0x00314338 File Offset: 0x00312738
		public PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.owner.GetPublishedFileId();
			}
			set
			{
				this.owner.SetPublishedFileId(value);
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x060061A1 RID: 24993 RVA: 0x00314348 File Offset: 0x00312748
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x060061A2 RID: 24994 RVA: 0x00314368 File Offset: 0x00312768
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x060061A3 RID: 24995 RVA: 0x00314388 File Offset: 0x00312788
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060061A4 RID: 24996 RVA: 0x003143A8 File Offset: 0x003127A8
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x060061A5 RID: 24997 RVA: 0x003143C8 File Offset: 0x003127C8
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x060061A6 RID: 24998 RVA: 0x003143E8 File Offset: 0x003127E8
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x060061A7 RID: 24999 RVA: 0x00314445 File Offset: 0x00312845
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x060061A8 RID: 25000 RVA: 0x00314454 File Offset: 0x00312854
		private void SendSteamDetailsQuery()
		{
			SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999u);
			this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
			this.queryResult.Set(hAPICall, null);
		}

		// Token: 0x060061A9 RID: 25001 RVA: 0x00314497 File Offset: 0x00312897
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		// Token: 0x04003FD0 RID: 16336
		private WorkshopUploadable owner;

		// Token: 0x04003FD1 RID: 16337
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x04003FD2 RID: 16338
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}

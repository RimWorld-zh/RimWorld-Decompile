using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC5 RID: 4037
	public class WorkshopItemHook
	{
		// Token: 0x06006177 RID: 24951 RVA: 0x00312130 File Offset: 0x00310530
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06006178 RID: 24952 RVA: 0x00312168 File Offset: 0x00310568
		// (set) Token: 0x06006179 RID: 24953 RVA: 0x00312188 File Offset: 0x00310588
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

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x0600617A RID: 24954 RVA: 0x00312198 File Offset: 0x00310598
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x0600617B RID: 24955 RVA: 0x003121B8 File Offset: 0x003105B8
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x0600617C RID: 24956 RVA: 0x003121D8 File Offset: 0x003105D8
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x0600617D RID: 24957 RVA: 0x003121F8 File Offset: 0x003105F8
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x0600617E RID: 24958 RVA: 0x00312218 File Offset: 0x00310618
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x0600617F RID: 24959 RVA: 0x00312238 File Offset: 0x00310638
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x00312295 File Offset: 0x00310695
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x003122A4 File Offset: 0x003106A4
		private void SendSteamDetailsQuery()
		{
			SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999u);
			this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
			this.queryResult.Set(hAPICall, null);
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x003122E7 File Offset: 0x003106E7
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}

		// Token: 0x04003FB4 RID: 16308
		private WorkshopUploadable owner;

		// Token: 0x04003FB5 RID: 16309
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x04003FB6 RID: 16310
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;
	}
}

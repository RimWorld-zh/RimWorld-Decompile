using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC9 RID: 4041
	public class WorkshopItemHook
	{
		// Token: 0x04003FD8 RID: 16344
		private WorkshopUploadable owner;

		// Token: 0x04003FD9 RID: 16345
		private CSteamID steamAuthor = CSteamID.Nil;

		// Token: 0x04003FDA RID: 16346
		private CallResult<SteamUGCRequestUGCDetailsResult_t> queryResult;

		// Token: 0x060061AE RID: 25006 RVA: 0x00314DC0 File Offset: 0x003131C0
		public WorkshopItemHook(WorkshopUploadable owner)
		{
			this.owner = owner;
			if (owner.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				this.SendSteamDetailsQuery();
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x060061AF RID: 25007 RVA: 0x00314DF8 File Offset: 0x003131F8
		// (set) Token: 0x060061B0 RID: 25008 RVA: 0x00314E18 File Offset: 0x00313218
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

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x060061B1 RID: 25009 RVA: 0x00314E28 File Offset: 0x00313228
		public string Name
		{
			get
			{
				return this.owner.GetWorkshopName();
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x060061B2 RID: 25010 RVA: 0x00314E48 File Offset: 0x00313248
		public string Description
		{
			get
			{
				return this.owner.GetWorkshopDescription();
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060061B3 RID: 25011 RVA: 0x00314E68 File Offset: 0x00313268
		public string PreviewImagePath
		{
			get
			{
				return this.owner.GetWorkshopPreviewImagePath();
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x060061B4 RID: 25012 RVA: 0x00314E88 File Offset: 0x00313288
		public IList<string> Tags
		{
			get
			{
				return this.owner.GetWorkshopTags();
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x060061B5 RID: 25013 RVA: 0x00314EA8 File Offset: 0x003132A8
		public DirectoryInfo Directory
		{
			get
			{
				return this.owner.GetWorkshopUploadDirectory();
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x060061B6 RID: 25014 RVA: 0x00314EC8 File Offset: 0x003132C8
		public bool MayHaveAuthorNotCurrentUser
		{
			get
			{
				return !(this.PublishedFileId == PublishedFileId_t.Invalid) && (this.steamAuthor == CSteamID.Nil || this.steamAuthor != SteamUser.GetSteamID());
			}
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x00314F25 File Offset: 0x00313325
		public void PrepareForWorkshopUpload()
		{
			this.owner.PrepareForWorkshopUpload();
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x00314F34 File Offset: 0x00313334
		private void SendSteamDetailsQuery()
		{
			SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(this.PublishedFileId, 999999u);
			this.queryResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnDetailsQueryReturned));
			this.queryResult.Set(hAPICall, null);
		}

		// Token: 0x060061B9 RID: 25017 RVA: 0x00314F77 File Offset: 0x00313377
		private void OnDetailsQueryReturned(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			this.steamAuthor = (CSteamID)result.m_details.m_ulSteamIDOwner;
		}
	}
}

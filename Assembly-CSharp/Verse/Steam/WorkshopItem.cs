using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FBF RID: 4031
	public class WorkshopItem
	{
		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x0600615E RID: 24926 RVA: 0x00311F98 File Offset: 0x00310398
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x0600615F RID: 24927 RVA: 0x00311FB4 File Offset: 0x003103B4
		// (set) Token: 0x06006160 RID: 24928 RVA: 0x00311FCF File Offset: 0x003103CF
		public virtual PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.pfidInt;
			}
			set
			{
				this.pfidInt = value;
			}
		}

		// Token: 0x06006161 RID: 24929 RVA: 0x00311FDC File Offset: 0x003103DC
		public static WorkshopItem MakeFrom(PublishedFileId_t pfid)
		{
			ulong num;
			string path;
			uint num2;
			bool itemInstallInfo = SteamUGC.GetItemInstallInfo(pfid, out num, out path, 257u, out num2);
			WorkshopItem workshopItem = null;
			if (!itemInstallInfo)
			{
				workshopItem = new WorkshopItem_NotInstalled();
			}
			else
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (!directoryInfo.Exists)
				{
					Log.Error("Created WorkshopItem for " + pfid + " but there is no folder for it.", false);
				}
				foreach (FileInfo fileInfo in directoryInfo.GetFiles())
				{
					if (fileInfo.Extension == ".rsc")
					{
						workshopItem = new WorkshopItem_Scenario();
						break;
					}
				}
				if (workshopItem == null)
				{
					workshopItem = new WorkshopItem_Mod();
				}
				workshopItem.directoryInt = directoryInfo;
			}
			workshopItem.PublishedFileId = pfid;
			return workshopItem;
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x003120C0 File Offset: 0x003104C0
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}

		// Token: 0x04003FAF RID: 16303
		protected DirectoryInfo directoryInt;

		// Token: 0x04003FB0 RID: 16304
		private PublishedFileId_t pfidInt;
	}
}

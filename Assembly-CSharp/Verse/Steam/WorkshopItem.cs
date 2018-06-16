using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC0 RID: 4032
	public class WorkshopItem
	{
		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06006160 RID: 24928 RVA: 0x00311EBC File Offset: 0x003102BC
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06006161 RID: 24929 RVA: 0x00311ED8 File Offset: 0x003102D8
		// (set) Token: 0x06006162 RID: 24930 RVA: 0x00311EF3 File Offset: 0x003102F3
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

		// Token: 0x06006163 RID: 24931 RVA: 0x00311F00 File Offset: 0x00310300
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

		// Token: 0x06006164 RID: 24932 RVA: 0x00311FE4 File Offset: 0x003103E4
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}

		// Token: 0x04003FB0 RID: 16304
		protected DirectoryInfo directoryInt;

		// Token: 0x04003FB1 RID: 16305
		private PublishedFileId_t pfidInt;
	}
}

using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC0 RID: 4032
	public class WorkshopItem
	{
		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06006187 RID: 24967 RVA: 0x0031406C File Offset: 0x0031246C
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06006188 RID: 24968 RVA: 0x00314088 File Offset: 0x00312488
		// (set) Token: 0x06006189 RID: 24969 RVA: 0x003140A3 File Offset: 0x003124A3
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

		// Token: 0x0600618A RID: 24970 RVA: 0x003140B0 File Offset: 0x003124B0
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

		// Token: 0x0600618B RID: 24971 RVA: 0x00314194 File Offset: 0x00312594
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}

		// Token: 0x04003FCC RID: 16332
		protected DirectoryInfo directoryInt;

		// Token: 0x04003FCD RID: 16333
		private PublishedFileId_t pfidInt;
	}
}

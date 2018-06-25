using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC5 RID: 4037
	public class WorkshopItem
	{
		// Token: 0x04003FDC RID: 16348
		protected DirectoryInfo directoryInt;

		// Token: 0x04003FDD RID: 16349
		private PublishedFileId_t pfidInt;

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06006197 RID: 24983 RVA: 0x00314D90 File Offset: 0x00313190
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06006198 RID: 24984 RVA: 0x00314DAC File Offset: 0x003131AC
		// (set) Token: 0x06006199 RID: 24985 RVA: 0x00314DC7 File Offset: 0x003131C7
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

		// Token: 0x0600619A RID: 24986 RVA: 0x00314DD4 File Offset: 0x003131D4
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

		// Token: 0x0600619B RID: 24987 RVA: 0x00314EB8 File Offset: 0x003132B8
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}
	}
}

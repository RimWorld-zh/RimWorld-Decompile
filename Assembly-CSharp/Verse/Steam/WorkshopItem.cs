using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC4 RID: 4036
	public class WorkshopItem
	{
		// Token: 0x04003FD4 RID: 16340
		protected DirectoryInfo directoryInt;

		// Token: 0x04003FD5 RID: 16341
		private PublishedFileId_t pfidInt;

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06006197 RID: 24983 RVA: 0x00314B4C File Offset: 0x00312F4C
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06006198 RID: 24984 RVA: 0x00314B68 File Offset: 0x00312F68
		// (set) Token: 0x06006199 RID: 24985 RVA: 0x00314B83 File Offset: 0x00312F83
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

		// Token: 0x0600619A RID: 24986 RVA: 0x00314B90 File Offset: 0x00312F90
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

		// Token: 0x0600619B RID: 24987 RVA: 0x00314C74 File Offset: 0x00313074
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}
	}
}

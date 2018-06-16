using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC2 RID: 4034
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06006167 RID: 24935 RVA: 0x0031202C File Offset: 0x0031042C
		// (set) Token: 0x06006168 RID: 24936 RVA: 0x00312047 File Offset: 0x00310447
		public override PublishedFileId_t PublishedFileId
		{
			get
			{
				return base.PublishedFileId;
			}
			set
			{
				base.PublishedFileId = value;
				if (this.cachedScenario != null)
				{
					this.cachedScenario.SetPublishedFileId(value);
				}
			}
		}

		// Token: 0x06006169 RID: 24937 RVA: 0x00312068 File Offset: 0x00310468
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x0600616A RID: 24938 RVA: 0x00312094 File Offset: 0x00310494
		private void LoadScenario()
		{
			FileInfo fileInfo = (from fi in base.Directory.GetFiles("*.rsc")
			where fi.Extension == ".rsc"
			select fi).First<FileInfo>();
			if (GameDataSaveLoader.TryLoadScenario(fileInfo.FullName, ScenarioCategory.SteamWorkshop, out this.cachedScenario))
			{
				this.cachedScenario.SetPublishedFileId(this.PublishedFileId);
			}
		}

		// Token: 0x04003FB2 RID: 16306
		private Scenario cachedScenario;
	}
}

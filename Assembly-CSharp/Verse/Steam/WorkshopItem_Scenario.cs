using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC1 RID: 4033
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06006165 RID: 24933 RVA: 0x00312108 File Offset: 0x00310508
		// (set) Token: 0x06006166 RID: 24934 RVA: 0x00312123 File Offset: 0x00310523
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

		// Token: 0x06006167 RID: 24935 RVA: 0x00312144 File Offset: 0x00310544
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x06006168 RID: 24936 RVA: 0x00312170 File Offset: 0x00310570
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

		// Token: 0x04003FB1 RID: 16305
		private Scenario cachedScenario;
	}
}

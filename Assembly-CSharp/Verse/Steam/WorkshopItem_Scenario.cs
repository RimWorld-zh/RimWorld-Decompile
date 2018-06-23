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
		// Token: 0x04003FCE RID: 16334
		private Scenario cachedScenario;

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x0600618E RID: 24974 RVA: 0x003141DC File Offset: 0x003125DC
		// (set) Token: 0x0600618F RID: 24975 RVA: 0x003141F7 File Offset: 0x003125F7
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

		// Token: 0x06006190 RID: 24976 RVA: 0x00314218 File Offset: 0x00312618
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x00314244 File Offset: 0x00312644
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
	}
}

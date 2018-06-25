using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC7 RID: 4039
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x04003FDE RID: 16350
		private Scenario cachedScenario;

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x0600619E RID: 24990 RVA: 0x00314F00 File Offset: 0x00313300
		// (set) Token: 0x0600619F RID: 24991 RVA: 0x00314F1B File Offset: 0x0031331B
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

		// Token: 0x060061A0 RID: 24992 RVA: 0x00314F3C File Offset: 0x0031333C
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x060061A1 RID: 24993 RVA: 0x00314F68 File Offset: 0x00313368
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

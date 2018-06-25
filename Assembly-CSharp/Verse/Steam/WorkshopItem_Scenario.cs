using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC6 RID: 4038
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x04003FD6 RID: 16342
		private Scenario cachedScenario;

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x0600619E RID: 24990 RVA: 0x00314CBC File Offset: 0x003130BC
		// (set) Token: 0x0600619F RID: 24991 RVA: 0x00314CD7 File Offset: 0x003130D7
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

		// Token: 0x060061A0 RID: 24992 RVA: 0x00314CF8 File Offset: 0x003130F8
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x060061A1 RID: 24993 RVA: 0x00314D24 File Offset: 0x00313124
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

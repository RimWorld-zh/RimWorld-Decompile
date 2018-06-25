using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	public class WorkshopItem_Scenario : WorkshopItem
	{
		private Scenario cachedScenario;

		[CompilerGenerated]
		private static Func<FileInfo, bool> <>f__am$cache0;

		public WorkshopItem_Scenario()
		{
		}

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

		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

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

		[CompilerGenerated]
		private static bool <LoadScenario>m__0(FileInfo fi)
		{
			return fi.Extension == ".rsc";
		}
	}
}

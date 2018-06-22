using System;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000868 RID: 2152
	public class MainButtonWorker_ToggleWorld : MainButtonWorker
	{
		// Token: 0x060030F7 RID: 12535 RVA: 0x001A9C5C File Offset: 0x001A805C
		public override void Activate()
		{
			if (Find.World.renderer.wantedMode == WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				if (this.resetViewNextTime)
				{
					this.resetViewNextTime = false;
					Find.World.UI.Reset();
				}
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.FormCaravan, OpportunityType.Important);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
			}
			else if (Find.MainTabsRoot.OpenTab != null && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
			}
			else
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x04001A78 RID: 6776
		public bool resetViewNextTime = true;
	}
}

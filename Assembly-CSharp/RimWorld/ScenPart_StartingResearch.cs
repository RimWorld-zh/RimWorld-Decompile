using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000646 RID: 1606
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x040012FD RID: 4861
		private ResearchProjectDef project;

		// Token: 0x0600215D RID: 8541 RVA: 0x0011B75C File Offset: 0x00119B5C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.project.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<ResearchProjectDef>(this.NonRedundantResearchProjects(), (ResearchProjectDef d) => d.LabelCap, (ResearchProjectDef d) => delegate()
				{
					this.project = d;
				});
			}
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x0011B7C6 File Offset: 0x00119BC6
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x0011B7DC File Offset: 0x00119BDC
		private IEnumerable<ResearchProjectDef> NonRedundantResearchProjects()
		{
			return DefDatabase<ResearchProjectDef>.AllDefs.Where(delegate(ResearchProjectDef d)
			{
				bool result;
				if (d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null)
				{
					result = true;
				}
				else
				{
					result = !d.tags.Any((ResearchProjectTagDef tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag));
				}
				return result;
			});
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x0011B818 File Offset: 0x00119C18
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x0011B834 File Offset: 0x00119C34
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(new object[]
			{
				this.project.LabelCap
			});
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x0011B867 File Offset: 0x00119C67
		public override void PostGameStart()
		{
			Find.ResearchManager.InstantFinish(this.project, false);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000648 RID: 1608
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x040012FD RID: 4861
		private ResearchProjectDef project;

		// Token: 0x06002161 RID: 8545 RVA: 0x0011B8AC File Offset: 0x00119CAC
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

		// Token: 0x06002162 RID: 8546 RVA: 0x0011B916 File Offset: 0x00119D16
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x0011B92C File Offset: 0x00119D2C
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

		// Token: 0x06002164 RID: 8548 RVA: 0x0011B968 File Offset: 0x00119D68
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0011B984 File Offset: 0x00119D84
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(new object[]
			{
				this.project.LabelCap
			});
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0011B9B7 File Offset: 0x00119DB7
		public override void PostGameStart()
		{
			Find.ResearchManager.InstantFinish(this.project, false);
		}
	}
}

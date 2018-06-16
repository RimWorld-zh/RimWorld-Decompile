using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064A RID: 1610
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x06002163 RID: 8547 RVA: 0x0011B5E4 File Offset: 0x001199E4
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

		// Token: 0x06002164 RID: 8548 RVA: 0x0011B64E File Offset: 0x00119A4E
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0011B664 File Offset: 0x00119A64
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

		// Token: 0x06002166 RID: 8550 RVA: 0x0011B6A0 File Offset: 0x00119AA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0011B6BC File Offset: 0x00119ABC
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(new object[]
			{
				this.project.LabelCap
			});
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0011B6EF File Offset: 0x00119AEF
		public override void PostGameStart()
		{
			Find.ResearchManager.InstantFinish(this.project, false);
		}

		// Token: 0x04001300 RID: 4864
		private ResearchProjectDef project;
	}
}

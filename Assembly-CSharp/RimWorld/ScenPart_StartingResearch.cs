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
		// Token: 0x06002165 RID: 8549 RVA: 0x0011B65C File Offset: 0x00119A5C
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

		// Token: 0x06002166 RID: 8550 RVA: 0x0011B6C6 File Offset: 0x00119AC6
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0011B6DC File Offset: 0x00119ADC
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

		// Token: 0x06002168 RID: 8552 RVA: 0x0011B718 File Offset: 0x00119B18
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0011B734 File Offset: 0x00119B34
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(new object[]
			{
				this.project.LabelCap
			});
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x0011B767 File Offset: 0x00119B67
		public override void PostGameStart()
		{
			Find.ResearchManager.InstantFinish(this.project, false);
		}

		// Token: 0x04001300 RID: 4864
		private ResearchProjectDef project;
	}
}

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
		// Token: 0x04001301 RID: 4865
		private ResearchProjectDef project;

		// Token: 0x06002160 RID: 8544 RVA: 0x0011BB14 File Offset: 0x00119F14
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

		// Token: 0x06002161 RID: 8545 RVA: 0x0011BB7E File Offset: 0x00119F7E
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x0011BB94 File Offset: 0x00119F94
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

		// Token: 0x06002163 RID: 8547 RVA: 0x0011BBD0 File Offset: 0x00119FD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x0011BBEC File Offset: 0x00119FEC
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(new object[]
			{
				this.project.LabelCap
			});
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0011BC1F File Offset: 0x0011A01F
		public override void PostGameStart()
		{
			Find.ResearchManager.InstantFinish(this.project, false);
		}
	}
}

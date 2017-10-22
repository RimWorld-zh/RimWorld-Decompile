using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_StartingResearch : ScenPart
	{
		private ResearchProjectDef project;

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.project.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu(this.NonRedundantResearchProjects(), (Func<ResearchProjectDef, string>)((ResearchProjectDef d) => d.LabelCap), (Func<ResearchProjectDef, Action>)((ResearchProjectDef d) => (Action)delegate()
				{
					this.project = d;
				}));
			}
		}

		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement();
		}

		private IEnumerable<ResearchProjectDef> NonRedundantResearchProjects()
		{
			return from d in DefDatabase<ResearchProjectDef>.AllDefs
			where d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null || !d.tags.Any((Predicate<string>)((string tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag)))
			select d;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(this.project.LabelCap);
		}

		public override void PostGameStart()
		{
			Find.ResearchManager.InstantFinish(this.project, false);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_StartingResearch : ScenPart
	{
		private ResearchProjectDef project;

		[CompilerGenerated]
		private static Func<ResearchProjectDef, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ResearchProjectDef, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<ResearchProjectTagDef> <>f__am$cache2;

		public ScenPart_StartingResearch()
		{
		}

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

		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		private IEnumerable<ResearchProjectDef> NonRedundantResearchProjects()
		{
			return DefDatabase<ResearchProjectDef>.AllDefs.Where(delegate(ResearchProjectDef d)
			{
				if (d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null)
				{
					return true;
				}
				return !d.tags.Any((ResearchProjectTagDef tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag));
			});
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(new object[]
			{
				this.project.LabelCap
			});
		}

		public override void PostGameStart()
		{
			Find.ResearchManager.FinishProject(this.project, false, null);
		}

		[CompilerGenerated]
		private static string <DoEditInterface>m__0(ResearchProjectDef d)
		{
			return d.LabelCap;
		}

		[CompilerGenerated]
		private Action <DoEditInterface>m__1(ResearchProjectDef d)
		{
			return delegate()
			{
				this.project = d;
			};
		}

		[CompilerGenerated]
		private static bool <NonRedundantResearchProjects>m__2(ResearchProjectDef d)
		{
			if (d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null)
			{
				return true;
			}
			return !d.tags.Any((ResearchProjectTagDef tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag));
		}

		[CompilerGenerated]
		private static bool <NonRedundantResearchProjects>m__3(ResearchProjectTagDef tag)
		{
			return Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag);
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey0
		{
			internal ResearchProjectDef d;

			internal ScenPart_StartingResearch $this;

			public <DoEditInterface>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.$this.project = this.d;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_DisallowBuilding : ScenPart_Rule
	{
		private ThingDef building;

		private const string DisallowBuildingTag = "DisallowBuilding";

		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowBuilding(this.building, false);
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
		}

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (!(tag == "DisallowBuilding"))
				yield break;
			yield return this.building.LabelCap;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.building, "building");
		}

		public override void Randomize()
		{
			this.building = this.RandomizableBuildingDefs().RandomElement();
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.building.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef item in from t in this.PossibleBuildingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = item;
					list.Add(new FloatMenuOption(localTd.LabelCap, (Action)delegate
					{
						this.building = localTd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = other as ScenPart_DisallowBuilding;
			return (byte)((scenPart_DisallowBuilding != null && scenPart_DisallowBuilding.building == this.building) ? 1 : 0) != 0;
		}

		protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.designationCategory != null
			select d;
		}

		private IEnumerable<ThingDef> RandomizableBuildingDefs()
		{
			yield return ThingDefOf.Wall;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}

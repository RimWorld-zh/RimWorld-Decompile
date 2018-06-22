using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000638 RID: 1592
	public class ScenPart_DisallowBuilding : ScenPart_Rule
	{
		// Token: 0x060020E8 RID: 8424 RVA: 0x0011914F File Offset: 0x0011754F
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowBuilding(this.building, false);
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00119168 File Offset: 0x00117568
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00119194 File Offset: 0x00117594
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "DisallowBuilding")
			{
				yield return this.building.LabelCap;
			}
			yield break;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x001191C5 File Offset: 0x001175C5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.building, "building");
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x001191DE File Offset: 0x001175DE
		public override void Randomize()
		{
			this.building = this.RandomizableBuildingDefs().RandomElement<ThingDef>();
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x001191F4 File Offset: 0x001175F4
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.building.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleBuildingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.building = localTd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x001192F0 File Offset: 0x001176F0
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = other as ScenPart_DisallowBuilding;
			return scenPart_DisallowBuilding != null && scenPart_DisallowBuilding.building == this.building;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x0011932C File Offset: 0x0011772C
		protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.BuildableByPlayer
			select d;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00119368 File Offset: 0x00117768
		private IEnumerable<ThingDef> RandomizableBuildingDefs()
		{
			yield return ThingDefOf.Wall;
			yield return ThingDefOf.Turret_MiniTurret;
			yield return ThingDefOf.OrbitalTradeBeacon;
			yield return ThingDefOf.Battery;
			yield return ThingDefOf.TrapDeadfall;
			yield return ThingDefOf.Cooler;
			yield return ThingDefOf.Heater;
			yield break;
		}

		// Token: 0x040012D4 RID: 4820
		private ThingDef building;

		// Token: 0x040012D5 RID: 4821
		private const string DisallowBuildingTag = "DisallowBuilding";
	}
}

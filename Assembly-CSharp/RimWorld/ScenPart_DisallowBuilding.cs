using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063A RID: 1594
	public class ScenPart_DisallowBuilding : ScenPart_Rule
	{
		// Token: 0x040012D8 RID: 4824
		private ThingDef building;

		// Token: 0x040012D9 RID: 4825
		private const string DisallowBuildingTag = "DisallowBuilding";

		// Token: 0x060020EB RID: 8427 RVA: 0x00119507 File Offset: 0x00117907
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowBuilding(this.building, false);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00119520 File Offset: 0x00117920
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x0011954C File Offset: 0x0011794C
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "DisallowBuilding")
			{
				yield return this.building.LabelCap;
			}
			yield break;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x0011957D File Offset: 0x0011797D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.building, "building");
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00119596 File Offset: 0x00117996
		public override void Randomize()
		{
			this.building = this.RandomizableBuildingDefs().RandomElement<ThingDef>();
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x001195AC File Offset: 0x001179AC
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

		// Token: 0x060020F1 RID: 8433 RVA: 0x001196A8 File Offset: 0x00117AA8
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = other as ScenPart_DisallowBuilding;
			return scenPart_DisallowBuilding != null && scenPart_DisallowBuilding.building == this.building;
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x001196E4 File Offset: 0x00117AE4
		protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.BuildableByPlayer
			select d;
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x00119720 File Offset: 0x00117B20
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
	}
}

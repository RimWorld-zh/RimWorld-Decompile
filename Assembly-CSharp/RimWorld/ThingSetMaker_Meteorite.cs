using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F7 RID: 1783
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x04001597 RID: 5527
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x04001598 RID: 5528
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x04001599 RID: 5529
		private const float PreciousMineableMarketValue = 5f;

		// Token: 0x060026D2 RID: 9938 RVA: 0x0014D751 File Offset: 0x0014BB51
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks && !x.IsSmoothed
			select x);
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0014D790 File Offset: 0x0014BB90
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IntRange? countRange = parms.countRange;
			int randomInRange = ((countRange == null) ? ThingSetMaker_Meteorite.MineablesCountRange : countRange.Value).RandomInRange;
			ThingDef def = this.FindRandomMineableDef();
			for (int i = 0; i < randomInRange; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(def, null);
				building.canChangeTerrainOnDestroyed = false;
				outThings.Add(building);
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x0014D80C File Offset: 0x0014BC0C
		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			ThingDef result;
			if (value < 0.4f)
			{
				result = (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where !x.building.isResourceRock
				select x).RandomElement<ThingDef>();
			}
			else if (value < 0.75f)
			{
				result = (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f
				select x).RandomElement<ThingDef>();
			}
			else
			{
				result = (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f
				select x).RandomElement<ThingDef>();
			}
			return result;
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x0014D8D0 File Offset: 0x0014BCD0
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}
	}
}

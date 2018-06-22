using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F5 RID: 1781
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x060026CF RID: 9935 RVA: 0x0014D3A1 File Offset: 0x0014B7A1
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks && !x.IsSmoothed
			select x);
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x0014D3E0 File Offset: 0x0014B7E0
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

		// Token: 0x060026D1 RID: 9937 RVA: 0x0014D45C File Offset: 0x0014B85C
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

		// Token: 0x060026D2 RID: 9938 RVA: 0x0014D520 File Offset: 0x0014B920
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Token: 0x04001593 RID: 5523
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x04001594 RID: 5524
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x04001595 RID: 5525
		private const float PreciousMineableMarketValue = 5f;
	}
}

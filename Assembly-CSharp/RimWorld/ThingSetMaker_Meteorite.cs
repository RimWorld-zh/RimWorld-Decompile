using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		private const float PreciousMineableMarketValue = 5f;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache3;

		public ThingSetMaker_Meteorite()
		{
		}

		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks && !x.IsSmoothed
			select x);
		}

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

		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			if (value < 0.4f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where !x.building.isResourceRock
				select x).RandomElement<ThingDef>();
			}
			if (value < 0.75f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f
				select x).RandomElement<ThingDef>();
			}
			return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
			where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f
			select x).RandomElement<ThingDef>();
		}

		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ThingSetMaker_Meteorite()
		{
		}

		[CompilerGenerated]
		private static bool <Reset>m__0(ThingDef x)
		{
			return x.mineable && x != ThingDefOf.CollapsedRocks && !x.IsSmoothed;
		}

		[CompilerGenerated]
		private static bool <FindRandomMineableDef>m__1(ThingDef x)
		{
			return !x.building.isResourceRock;
		}

		[CompilerGenerated]
		private static bool <FindRandomMineableDef>m__2(ThingDef x)
		{
			return x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f;
		}

		[CompilerGenerated]
		private static bool <FindRandomMineableDef>m__3(ThingDef x)
		{
			return x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f;
		}
	}
}

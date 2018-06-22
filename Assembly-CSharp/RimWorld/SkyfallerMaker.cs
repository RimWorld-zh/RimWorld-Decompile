using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E6 RID: 1766
	public static class SkyfallerMaker
	{
		// Token: 0x06002680 RID: 9856 RVA: 0x00149F54 File Offset: 0x00148354
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x00149F78 File Offset: 0x00148378
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x00149F9C File Offset: 0x0014839C
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, Thing innerThing)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (innerThing != null && !skyfaller2.innerContainer.TryAdd(innerThing, true))
			{
				Log.Error("Could not add " + innerThing.ToStringSafe<Thing>() + " to a skyfaller.", false);
				innerThing.Destroy(DestroyMode.Vanish);
			}
			return skyfaller2;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x00149FF8 File Offset: 0x001483F8
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0014A02C File Offset: 0x0014842C
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0014A058 File Offset: 0x00148458
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0014A084 File Offset: 0x00148484
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0014A0B0 File Offset: 0x001484B0
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, things);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}
	}
}

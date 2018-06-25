using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E8 RID: 1768
	public static class SkyfallerMaker
	{
		// Token: 0x06002683 RID: 9859 RVA: 0x0014A304 File Offset: 0x00148704
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0014A328 File Offset: 0x00148728
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0014A34C File Offset: 0x0014874C
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

		// Token: 0x06002686 RID: 9862 RVA: 0x0014A3A8 File Offset: 0x001487A8
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0014A3DC File Offset: 0x001487DC
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0014A408 File Offset: 0x00148808
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0014A434 File Offset: 0x00148834
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0014A460 File Offset: 0x00148860
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, things);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}
	}
}

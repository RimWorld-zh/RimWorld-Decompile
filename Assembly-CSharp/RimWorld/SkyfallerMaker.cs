using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E8 RID: 1768
	public static class SkyfallerMaker
	{
		// Token: 0x06002684 RID: 9860 RVA: 0x0014A0A4 File Offset: 0x001484A4
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0014A0C8 File Offset: 0x001484C8
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0014A0EC File Offset: 0x001484EC
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

		// Token: 0x06002687 RID: 9863 RVA: 0x0014A148 File Offset: 0x00148548
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0014A17C File Offset: 0x0014857C
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0014A1A8 File Offset: 0x001485A8
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0014A1D4 File Offset: 0x001485D4
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x0014A200 File Offset: 0x00148600
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, things);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006EA RID: 1770
	public static class SkyfallerMaker
	{
		// Token: 0x06002686 RID: 9862 RVA: 0x00149D38 File Offset: 0x00148138
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x00149D5C File Offset: 0x0014815C
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x00149D80 File Offset: 0x00148180
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

		// Token: 0x06002689 RID: 9865 RVA: 0x00149DDC File Offset: 0x001481DC
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x00149E10 File Offset: 0x00148210
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x00149E3C File Offset: 0x0014823C
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x00149E68 File Offset: 0x00148268
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x00149E94 File Offset: 0x00148294
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			Skyfaller newThing = SkyfallerMaker.MakeSkyfaller(skyfaller, things);
			return (Skyfaller)GenSpawn.Spawn(newThing, pos, map, WipeMode.Vanish);
		}
	}
}

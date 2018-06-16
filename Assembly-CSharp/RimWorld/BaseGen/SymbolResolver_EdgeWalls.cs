using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003AC RID: 940
	public class SymbolResolver_EdgeWalls : SymbolResolver
	{
		// Token: 0x06001051 RID: 4177 RVA: 0x00089AB4 File Offset: 0x00087EB4
		public override void Resolve(ResolveParams rp)
		{
			ThingDef wallStuff = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			foreach (IntVec3 c in rp.rect.EdgeCells)
			{
				this.TrySpawnWall(c, rp, wallStuff);
			}
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00089B38 File Offset: 0x00087F38
		private Thing TrySpawnWall(IntVec3 c, ResolveParams rp, ThingDef wallStuff)
		{
			Map map = BaseGen.globalSettings.map;
			List<Thing> thingList = c.GetThingList(map);
			int i = 0;
			while (i < thingList.Count)
			{
				Thing result;
				if (!thingList[i].def.destroyable)
				{
					result = null;
				}
				else
				{
					if (!(thingList[i] is Building_Door))
					{
						i++;
						continue;
					}
					result = null;
				}
				return result;
			}
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				thingList[j].Destroy(DestroyMode.Vanish);
			}
			if (rp.chanceToSkipWallBlock != null && Rand.Chance(rp.chanceToSkipWallBlock.Value))
			{
				return null;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, wallStuff);
			thing.SetFaction(rp.faction, null);
			return GenSpawn.Spawn(thing, c, map, WipeMode.Vanish);
		}
	}
}

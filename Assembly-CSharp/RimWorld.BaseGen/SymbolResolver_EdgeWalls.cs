using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EdgeWalls : SymbolResolver
	{
		public override void Resolve(ResolveParams rp)
		{
			ThingDef wallStuff = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			foreach (IntVec3 edgeCell in rp.rect.EdgeCells)
			{
				this.TrySpawnWall(edgeCell, rp, wallStuff);
			}
		}

		private Thing TrySpawnWall(IntVec3 c, ResolveParams rp, ThingDef wallStuff)
		{
			Map map = BaseGen.globalSettings.map;
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (!thingList[num].def.destroyable)
					{
						result = null;
						break;
					}
					if (thingList[num] is Building_Door)
					{
						result = null;
						break;
					}
					num++;
					continue;
				}
				for (int num2 = thingList.Count - 1; num2 >= 0; num2--)
				{
					thingList[num2].Destroy(DestroyMode.Vanish);
				}
				if (rp.chanceToSkipWallBlock.HasValue && Rand.Chance(rp.chanceToSkipWallBlock.Value))
				{
					result = null;
				}
				else
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, wallStuff);
					thing.SetFaction(rp.faction, null);
					result = GenSpawn.Spawn(thing, c, map);
				}
				break;
			}
			return result;
		}
	}
}

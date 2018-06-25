using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C2 RID: 1730
	public static class FilthMaker
	{
		// Token: 0x040014BF RID: 5311
		private static List<Filth> toBeRemoved = new List<Filth>();

		// Token: 0x06002554 RID: 9556 RVA: 0x0014037C File Offset: 0x0013E77C
		public static void MakeFilth(IntVec3 c, Map map, ThingDef filthDef, int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				FilthMaker.MakeFilth(c, map, filthDef, null, true);
			}
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x001403AC File Offset: 0x0013E7AC
		public static bool MakeFilth(IntVec3 c, Map map, ThingDef filthDef, string source, int count = 1)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= FilthMaker.MakeFilth(c, map, filthDef, Gen.YieldSingle<string>(source), true);
			}
			return flag;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x001403EB File Offset: 0x0013E7EB
		public static void MakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources)
		{
			FilthMaker.MakeFilth(c, map, filthDef, sources, true);
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x001403FC File Offset: 0x0013E7FC
		private static bool MakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources, bool shouldPropagate)
		{
			Filth filth = (Filth)(from t in c.GetThingList(map)
			where t.def == filthDef
			select t).FirstOrDefault<Thing>();
			bool result;
			if (!c.Walkable(map) || (filth != null && !filth.CanBeThickened))
			{
				if (shouldPropagate)
				{
					List<IntVec3> list = GenAdj.AdjacentCells8WayRandomized();
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c2 = c + list[i];
						if (c2.InBounds(map))
						{
							if (FilthMaker.MakeFilth(c2, map, filthDef, sources, false))
							{
								return true;
							}
						}
					}
				}
				if (filth != null)
				{
					filth.AddSources(sources);
				}
				result = false;
			}
			else
			{
				if (filth != null)
				{
					filth.ThickenFilth();
					filth.AddSources(sources);
				}
				else
				{
					Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
					filth2.AddSources(sources);
					GenSpawn.Spawn(filth2, c, map, WipeMode.Vanish);
				}
				FilthMonitor.Notify_FilthSpawned(filthDef);
				result = true;
			}
			return result;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x00140528 File Offset: 0x0013E928
		public static void RemoveAllFilth(IntVec3 c, Map map)
		{
			FilthMaker.toBeRemoved.Clear();
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Filth filth = thingList[i] as Filth;
				if (filth != null)
				{
					FilthMaker.toBeRemoved.Add(filth);
				}
			}
			for (int j = 0; j < FilthMaker.toBeRemoved.Count; j++)
			{
				FilthMaker.toBeRemoved[j].Destroy(DestroyMode.Vanish);
			}
			FilthMaker.toBeRemoved.Clear();
		}
	}
}

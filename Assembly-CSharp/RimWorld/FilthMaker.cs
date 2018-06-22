using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C0 RID: 1728
	public static class FilthMaker
	{
		// Token: 0x06002550 RID: 9552 RVA: 0x0014022C File Offset: 0x0013E62C
		public static void MakeFilth(IntVec3 c, Map map, ThingDef filthDef, int count = 1)
		{
			for (int i = 0; i < count; i++)
			{
				FilthMaker.MakeFilth(c, map, filthDef, null, true);
			}
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x0014025C File Offset: 0x0013E65C
		public static bool MakeFilth(IntVec3 c, Map map, ThingDef filthDef, string source, int count = 1)
		{
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				flag |= FilthMaker.MakeFilth(c, map, filthDef, Gen.YieldSingle<string>(source), true);
			}
			return flag;
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x0014029B File Offset: 0x0013E69B
		public static void MakeFilth(IntVec3 c, Map map, ThingDef filthDef, IEnumerable<string> sources)
		{
			FilthMaker.MakeFilth(c, map, filthDef, sources, true);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x001402AC File Offset: 0x0013E6AC
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

		// Token: 0x06002554 RID: 9556 RVA: 0x001403D8 File Offset: 0x0013E7D8
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

		// Token: 0x040014BF RID: 5311
		private static List<Filth> toBeRemoved = new List<Filth>();
	}
}

using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class FireUtility
	{
		public static bool CanEverAttachFire(this Thing t)
		{
			return (byte)((!t.Destroyed) ? (t.FlammableNow ? ((t.def.category == ThingCategory.Pawn) ? 1 : 0) : 0) : 0) != 0;
		}

		public static float ChanceToStartFireIn(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			float num = (float)((!c.TerrainFlammableNow(map)) ? 0.0 : c.GetTerrain(map).GetStatValueAbstract(StatDefOf.Flammability, null));
			int num2 = 0;
			float result;
			while (true)
			{
				if (num2 < thingList.Count)
				{
					if (thingList[num2] is Fire)
					{
						result = 0f;
						break;
					}
					if (thingList[num2].FlammableNow)
					{
						num = 1f;
					}
					num2++;
					continue;
				}
				if (num > 0.0)
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.passability == Traversability.Impassable && edifice.OccupiedRect().ContractedBy(1).Contains(c))
					{
						result = 0f;
						break;
					}
					List<Thing> thingList2 = c.GetThingList(map);
					for (int i = 0; i < thingList2.Count; i++)
					{
						if (thingList2[i].def.category == ThingCategory.Filth && !thingList2[i].def.filth.allowsFire)
							goto IL_0121;
					}
				}
				result = num;
				break;
				IL_0121:
				result = 0f;
				break;
			}
			return result;
		}

		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize)
		{
			float num = FireUtility.ChanceToStartFireIn(c, map);
			bool result;
			if (num <= 0.0)
			{
				result = false;
			}
			else
			{
				Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
				fire.fireSize = fireSize;
				GenSpawn.Spawn(fire, c, map, Rot4.North, false);
				result = true;
			}
			return result;
		}

		public static void TryAttachFire(this Thing t, float fireSize)
		{
			if (t.CanEverAttachFire() && !t.HasAttachment(ThingDefOf.Fire))
			{
				Fire fire = ThingMaker.MakeThing(ThingDefOf.Fire, null) as Fire;
				fire.fireSize = fireSize;
				fire.AttachTo(t);
				GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, false);
				Pawn pawn = t as Pawn;
				if (pawn != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					pawn.records.Increment(RecordDefOf.TimesOnFire);
				}
			}
		}

		public static bool IsBurning(this TargetInfo t)
		{
			return (!t.HasThing) ? t.Cell.ContainsStaticFire(t.Map) : t.Thing.IsBurning();
		}

		public static bool IsBurning(this Thing t)
		{
			bool result;
			if (t.Destroyed || !t.Spawned)
			{
				result = false;
			}
			else if (t.def.size == IntVec2.One)
			{
				result = ((!(t is Pawn)) ? t.Position.ContainsStaticFire(t.Map) : t.HasAttachment(ThingDefOf.Fire));
			}
			else
			{
				CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.ContainsStaticFire(t.Map))
						goto IL_0099;
					iterator.MoveNext();
				}
				result = false;
			}
			goto IL_00bc;
			IL_00bc:
			return result;
			IL_0099:
			result = true;
			goto IL_00bc;
		}

		public static bool ContainsStaticFire(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < list.Count)
				{
					Fire fire = list[num] as Fire;
					if (fire != null && fire.parent == null)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.0099999997764825821;
		}

		public static bool TerrainFlammableNow(this IntVec3 c, Map map)
		{
			TerrainDef terrain = c.GetTerrain(map);
			bool result;
			if (!terrain.Flammable())
			{
				result = false;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].FireBulwark)
						goto IL_003c;
				}
				result = true;
			}
			goto IL_005b;
			IL_003c:
			result = false;
			goto IL_005b;
			IL_005b:
			return result;
		}
	}
}

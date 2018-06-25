using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class FireUtility
	{
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		public static float ChanceToStartFireIn(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			float num = (!c.TerrainFlammableNow(map)) ? 0f : c.GetTerrain(map).GetStatValueAbstract(StatDefOf.Flammability, null);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is Fire)
				{
					return 0f;
				}
				if (thing.def.category != ThingCategory.Pawn && thingList[i].FlammableNow)
				{
					num = Mathf.Max(num, thing.GetStatValue(StatDefOf.Flammability, true));
				}
			}
			if (num > 0f)
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.passability == Traversability.Impassable)
				{
					if (edifice.OccupiedRect().ContractedBy(1).Contains(c))
					{
						return 0f;
					}
				}
				List<Thing> thingList2 = c.GetThingList(map);
				for (int j = 0; j < thingList2.Count; j++)
				{
					if (thingList2[j].def.category == ThingCategory.Filth && !thingList2[j].def.filth.allowsFire)
					{
						return 0f;
					}
				}
			}
			return num;
		}

		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize)
		{
			float num = FireUtility.ChanceToStartFireIn(c, map);
			bool result;
			if (num <= 0f)
			{
				result = false;
			}
			else
			{
				Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
				fire.fireSize = fireSize;
				GenSpawn.Spawn(fire, c, map, Rot4.North, WipeMode.Vanish, false);
				result = true;
			}
			return result;
		}

		public static void TryAttachFire(this Thing t, float fireSize)
		{
			if (t.CanEverAttachFire())
			{
				if (!t.HasAttachment(ThingDefOf.Fire))
				{
					Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
					fire.fireSize = fireSize;
					fire.AttachTo(t);
					GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, WipeMode.Vanish, false);
					Pawn pawn = t as Pawn;
					if (pawn != null)
					{
						pawn.jobs.StopAll(false);
						pawn.records.Increment(RecordDefOf.TimesOnFire);
					}
				}
			}
		}

		public static bool IsBurning(this TargetInfo t)
		{
			bool result;
			if (t.HasThing)
			{
				result = t.Thing.IsBurning();
			}
			else
			{
				result = t.Cell.ContainsStaticFire(t.Map);
			}
			return result;
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
				if (t is Pawn)
				{
					result = t.HasAttachment(ThingDefOf.Fire);
				}
				else
				{
					result = t.Position.ContainsStaticFire(t.Map);
				}
			}
			else
			{
				CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.ContainsStaticFire(t.Map))
					{
						return true;
					}
					iterator.MoveNext();
				}
				result = false;
			}
			return result;
		}

		public static bool ContainsStaticFire(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					return true;
				}
			}
			return false;
		}

		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
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
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}
	}
}

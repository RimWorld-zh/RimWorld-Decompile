using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C4 RID: 1732
	public static class FireUtility
	{
		// Token: 0x0600256B RID: 9579 RVA: 0x00141524 File Offset: 0x0013F924
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x00141588 File Offset: 0x0013F988
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

		// Token: 0x0600256D RID: 9581 RVA: 0x00141708 File Offset: 0x0013FB08
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

		// Token: 0x0600256E RID: 9582 RVA: 0x00141760 File Offset: 0x0013FB60
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

		// Token: 0x0600256F RID: 9583 RVA: 0x001417F8 File Offset: 0x0013FBF8
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

		// Token: 0x06002570 RID: 9584 RVA: 0x00141840 File Offset: 0x0013FC40
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

		// Token: 0x06002571 RID: 9585 RVA: 0x0014190C File Offset: 0x0013FD0C
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

		// Token: 0x06002572 RID: 9586 RVA: 0x00141970 File Offset: 0x0013FD70
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x001419A0 File Offset: 0x0013FDA0
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x001419C8 File Offset: 0x0013FDC8
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

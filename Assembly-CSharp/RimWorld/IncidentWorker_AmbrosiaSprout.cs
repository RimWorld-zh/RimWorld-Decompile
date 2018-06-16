using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000322 RID: 802
	public class IncidentWorker_AmbrosiaSprout : IncidentWorker
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x00075334 File Offset: 0x00073734
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				IntVec3 intVec;
				result = (map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow && this.TryFindRootCell(map, out intVec));
			}
			return result;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00075390 File Offset: 0x00073790
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 root;
			bool result;
			if (!this.TryFindRootCell(map, out root))
			{
				result = false;
			}
			else
			{
				Thing thing = null;
				int randomInRange = IncidentWorker_AmbrosiaSprout.CountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					IntVec3 intVec;
					if (!CellFinder.TryRandomClosewalkCellNear(root, map, 6, out intVec, (IntVec3 x) => this.CanSpawnAt(x, map)))
					{
						break;
					}
					Plant plant = intVec.GetPlant(map);
					if (plant != null)
					{
						plant.Destroy(DestroyMode.Vanish);
					}
					Thing thing2 = GenSpawn.Spawn(ThingDefOf.Plant_Ambrosia, intVec, map, WipeMode.Vanish);
					if (thing == null)
					{
						thing = thing2;
					}
				}
				if (thing == null)
				{
					result = false;
				}
				else
				{
					base.SendStandardLetter(thing, null, new string[0]);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0007548C File Offset: 0x0007388C
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 64, map, out cell);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000754D0 File Offset: 0x000738D0
		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			bool result;
			if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < ThingDefOf.Plant_Ambrosia.plant.fertilityMin || !c.GetRoom(map, RegionType.Set_Passable).PsychologicallyOutdoors || c.GetEdifice(map) != null || !GenPlant.GrowthSeasonNow(c, map, false))
			{
				result = false;
			}
			else
			{
				Plant plant = c.GetPlant(map);
				if (plant != null && plant.def.plant.growDays > 10f)
				{
					result = false;
				}
				else
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def == ThingDefOf.Plant_Ambrosia)
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x040008BD RID: 2237
		private static readonly IntRange CountRange = new IntRange(10, 20);

		// Token: 0x040008BE RID: 2238
		private const int MinRoomCells = 64;

		// Token: 0x040008BF RID: 2239
		private const int SpawnRadius = 6;
	}
}

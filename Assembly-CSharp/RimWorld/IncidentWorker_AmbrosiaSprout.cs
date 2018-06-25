using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000324 RID: 804
	public class IncidentWorker_AmbrosiaSprout : IncidentWorker
	{
		// Token: 0x040008BF RID: 2239
		private static readonly IntRange CountRange = new IntRange(10, 20);

		// Token: 0x040008C0 RID: 2240
		private const int MinRoomCells = 64;

		// Token: 0x040008C1 RID: 2241
		private const int SpawnRadius = 6;

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00075538 File Offset: 0x00073938
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

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00075594 File Offset: 0x00073994
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

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00075690 File Offset: 0x00073A90
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 64, map, out cell);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x000756D4 File Offset: 0x00073AD4
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
	}
}

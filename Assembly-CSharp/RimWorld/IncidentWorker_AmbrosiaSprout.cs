using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_AmbrosiaSprout : IncidentWorker
	{
		private static readonly IntRange CountRange = new IntRange(10, 20);

		private const int MinRoomCells = 64;

		private const int SpawnRadius = 6;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			bool result;
			if (!base.CanFireNowSub(target))
			{
				result = false;
			}
			else
			{
				Map map = (Map)target;
				IntVec3 intVec = default(IntVec3);
				result = (map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow && this.TryFindRootCell(map, out intVec));
			}
			return result;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 root = default(IntVec3);
			bool result;
			if (!this.TryFindRootCell(map, out root))
			{
				result = false;
			}
			else
			{
				Thing thing = null;
				int randomInRange = IncidentWorker_AmbrosiaSprout.CountRange.RandomInRange;
				int num = 0;
				IntVec3 intVec = default(IntVec3);
				while (num < randomInRange && CellFinder.TryRandomClosewalkCellNear(root, map, 6, out intVec, (Predicate<IntVec3>)((IntVec3 x) => this.CanSpawnAt(x, map))))
				{
					Plant plant = intVec.GetPlant(map);
					if (plant != null)
					{
						plant.Destroy(DestroyMode.Vanish);
					}
					Thing thing2 = GenSpawn.Spawn(ThingDefOf.PlantAmbrosia, intVec, map);
					if (thing == null)
					{
						thing = thing2;
					}
					num++;
				}
				if (thing == null)
				{
					result = false;
				}
				else
				{
					base.SendStandardLetter(thing);
					result = true;
				}
			}
			return result;
		}

		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (Predicate<IntVec3>)((IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 64), map, out cell);
		}

		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			bool result;
			if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < ThingDefOf.PlantAmbrosia.plant.fertilityMin || !c.GetRoom(map, RegionType.Set_Passable).PsychologicallyOutdoors || c.GetEdifice(map) != null || !GenPlant.GrowthSeasonNow(c, map))
			{
				result = false;
			}
			else
			{
				Plant plant = c.GetPlant(map);
				if (plant != null && plant.def.plant.growDays > 10.0)
				{
					result = false;
				}
				else
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def == ThingDefOf.PlantAmbrosia)
							goto IL_00c0;
					}
					result = true;
				}
			}
			goto IL_00df;
			IL_00c0:
			result = false;
			goto IL_00df;
			IL_00df:
			return result;
		}
	}
}

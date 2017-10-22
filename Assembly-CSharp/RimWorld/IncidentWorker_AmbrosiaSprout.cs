using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_AmbrosiaSprout : IncidentWorker
	{
		private const int MinRoomCells = 64;

		private const int SpawnRadius = 6;

		private static readonly IntRange CountRange = new IntRange(10, 20);

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (!base.CanFireNowSub(target))
			{
				return false;
			}
			Map map = (Map)target;
			if (!map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow)
			{
				return false;
			}
			IntVec3 intVec = default(IntVec3);
			return this.TryFindRootCell(map, out intVec);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 root = default(IntVec3);
			if (!this.TryFindRootCell(map, out root))
			{
				return false;
			}
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
				return false;
			}
			base.SendStandardLetter(thing);
			return true;
		}

		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (Predicate<IntVec3>)((IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 64), map, out cell);
		}

		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			if (c.Standable(map) && !c.Fogged(map) && !(map.fertilityGrid.FertilityAt(c) < ThingDefOf.PlantAmbrosia.plant.fertilityMin) && c.GetRoom(map, RegionType.Set_Passable).PsychologicallyOutdoors && c.GetEdifice(map) == null && GenPlant.GrowthSeasonNow(c, map))
			{
				Plant plant = c.GetPlant(map);
				if (plant != null && plant.def.plant.growDays > 10.0)
				{
					return false;
				}
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def == ThingDefOf.PlantAmbrosia)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_AmbrosiaSprout : IncidentWorker
	{
		private static readonly IntRange CountRange = new IntRange(10, 20);

		private const int MinRoomCells = 64;

		private const int SpawnRadius = 6;

		public IncidentWorker_AmbrosiaSprout()
		{
		}

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

		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 64, map, out cell);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_AmbrosiaSprout()
		{
		}

		[CompilerGenerated]
		private sealed class <TryExecuteWorker>c__AnonStorey0
		{
			internal Map map;

			internal IncidentWorker_AmbrosiaSprout $this;

			public <TryExecuteWorker>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return this.$this.CanSpawnAt(x, this.map);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRootCell>c__AnonStorey1
		{
			internal Map map;

			internal IncidentWorker_AmbrosiaSprout $this;

			public <TryFindRootCell>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return this.$this.CanSpawnAt(x, this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= 64;
			}
		}
	}
}

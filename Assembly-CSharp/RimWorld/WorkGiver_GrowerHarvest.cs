using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_GrowerHarvest : WorkGiver_Grower
	{
		public WorkGiver_GrowerHarvest()
		{
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Plant plant = c.GetPlant(pawn.Map);
			bool result;
			if (plant == null)
			{
				result = false;
			}
			else if (plant.IsForbidden(pawn))
			{
				result = false;
			}
			else if (!plant.HarvestableNow || plant.LifeStage != PlantLifeStage.Mature)
			{
				result = false;
			}
			else if (!plant.CanYieldNow())
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = plant;
				result = pawn.CanReserve(target, 1, -1, null, forced);
			}
			return result;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = new Job(JobDefOf.Harvest);
			Map map = pawn.Map;
			Room room = c.GetRoom(map, RegionType.Set_Passable);
			float num = 0f;
			for (int i = 0; i < 40; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.GetRoom(map, RegionType.Set_Passable) == room)
				{
					if (this.HasJobOnCell(pawn, intVec, false))
					{
						Plant plant = intVec.GetPlant(map);
						if (!(intVec != c) || plant.def == WorkGiver_Grower.CalculateWantedPlantDef(intVec, map))
						{
							num += plant.def.plant.harvestWork;
							if (intVec != c && num > 2400f)
							{
								break;
							}
							job.AddQueuedTarget(TargetIndex.A, plant);
						}
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 3)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}

		[CompilerGenerated]
		private sealed class <JobOnCell>c__AnonStorey0
		{
			internal Pawn pawn;

			public <JobOnCell>c__AnonStorey0()
			{
			}

			internal int <>m__0(LocalTargetInfo targ)
			{
				return targ.Cell.DistanceToSquared(this.pawn.Position);
			}
		}
	}
}

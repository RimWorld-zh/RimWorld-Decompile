using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_GrowerHarvest : WorkGiver_Grower
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c)
		{
			Plant plant = c.GetPlant(pawn.Map);
			if (plant == null)
			{
				return false;
			}
			if (plant.IsForbidden(pawn))
			{
				return false;
			}
			if (plant.HarvestableNow && plant.LifeStage == PlantLifeStage.Mature)
			{
				if (plant.YieldNow() <= 0)
				{
					return false;
				}
				if (!pawn.CanReserve(plant, 1, -1, null, false))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c)
		{
			Job job = new Job(JobDefOf.Harvest);
			Map map = pawn.Map;
			Room room = c.GetRoom(map, RegionType.Set_Passable);
			float num = 0f;
			for (int i = 0; i < 40; i++)
			{
				IntVec3 c2 = c + GenRadial.RadialPattern[i];
				if (c.GetRoom(map, RegionType.Set_Passable) == room && this.HasJobOnCell(pawn, c2))
				{
					Plant plant = c2.GetPlant(map);
					num += plant.def.plant.harvestWork;
					if (!(num > 2400.0))
					{
						job.AddQueuedTarget(TargetIndex.A, plant);
						continue;
					}
					break;
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 3)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}
	}
}

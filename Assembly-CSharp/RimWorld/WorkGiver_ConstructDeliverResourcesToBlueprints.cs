using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Blueprint blueprint = t as Blueprint;
			if (blueprint == null)
			{
				return null;
			}
			Thing thingToIgnore = GenConstruct.MiniToInstallOrBuildingToReinstall(blueprint);
			Thing thing = blueprint.FirstBlockingThing(pawn, thingToIgnore, false);
			if (thing != null)
			{
				if (thing.def.category == ThingCategory.Plant)
				{
					if (pawn.CanReserveAndReach(thing, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), 1, -1, null, forced))
					{
						return new Job(JobDefOf.CutPlant, thing);
					}
				}
				else if (thing.def.category == ThingCategory.Item)
				{
					if (thing.def.EverHaulable)
					{
						return HaulAIUtility.HaulAsideJobFor(pawn, thing);
					}
					Log.ErrorOnce("Never haulable " + thing + " blocking " + t + " at " + t.Position, 6429262);
				}
				return null;
			}
			if (!GenConstruct.CanConstruct(blueprint, pawn, forced))
			{
				return null;
			}
			Job job = this.DeconstructExistingBuildingJob(pawn, blueprint);
			if (job != null)
			{
				return job;
			}
			Job job2 = base.RemoveExistingFloorJob(pawn, blueprint);
			if (job2 != null)
			{
				return job2;
			}
			Job job3 = base.ResourceDeliverJobFor(pawn, blueprint, true);
			if (job3 != null)
			{
				return job3;
			}
			Job job4 = this.NoCostFrameMakeJobFor(pawn, blueprint);
			if (job4 != null)
			{
				return job4;
			}
			return null;
		}

		private Job DeconstructExistingBuildingJob(Pawn pawn, Blueprint blue)
		{
			Thing thing = GenConstruct.MiniToInstallOrBuildingToReinstall(blue);
			Thing thing2 = null;
			CellRect cellRect = blue.OccupiedRect();
			int num = cellRect.minZ;
			while (num <= cellRect.maxZ)
			{
				int num2 = cellRect.minX;
				while (num2 <= cellRect.maxX)
				{
					IntVec3 c = new IntVec3(num2, 0, num);
					List<Thing> thingList = c.GetThingList(pawn.Map);
					int num3 = 0;
					while (num3 < thingList.Count)
					{
						if (thingList[num3].def.category != ThingCategory.Building || thingList[num3] == thing || !GenSpawn.SpawningWipes(blue.def.entityDefToBuild, thingList[num3].def))
						{
							num3++;
							continue;
						}
						thing2 = thingList[num3];
						break;
					}
					if (thing2 == null)
					{
						num2++;
						continue;
					}
					break;
				}
				if (thing2 == null)
				{
					num++;
					continue;
				}
				break;
			}
			if (thing2 != null && pawn.CanReserve(thing2, 1, -1, null, false))
			{
				Job job = new Job(JobDefOf.Deconstruct, thing2);
				job.ignoreDesignations = true;
				return job;
			}
			return null;
		}

		private Job NoCostFrameMakeJobFor(Pawn pawn, IConstructible c)
		{
			if (c is Blueprint_Install)
			{
				return null;
			}
			if (c is Blueprint && c.MaterialsNeeded().Count == 0)
			{
				Job job = new Job(JobDefOf.PlaceNoCostFrame);
				job.targetA = (Thing)c;
				return job;
			}
			return null;
		}
	}
}

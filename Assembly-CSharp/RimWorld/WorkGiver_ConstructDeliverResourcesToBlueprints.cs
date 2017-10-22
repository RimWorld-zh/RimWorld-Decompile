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
			Job result;
			if (t.Faction != pawn.Faction)
			{
				result = null;
			}
			else
			{
				Blueprint blueprint = t as Blueprint;
				if (blueprint == null)
				{
					result = null;
				}
				else
				{
					Thing thingToIgnore = GenConstruct.MiniToInstallOrBuildingToReinstall(blueprint);
					Thing thing = blueprint.FirstBlockingThing(pawn, thingToIgnore, false);
					if (thing != null)
					{
						if (thing.def.category == ThingCategory.Plant)
						{
							LocalTargetInfo target = thing;
							PathEndMode peMode = PathEndMode.ClosestTouch;
							Danger maxDanger = pawn.NormalMaxDanger();
							if (pawn.CanReserveAndReach(target, peMode, maxDanger, 1, -1, null, forced))
							{
								result = new Job(JobDefOf.CutPlant, thing);
								goto IL_019d;
							}
						}
						else if (thing.def.category == ThingCategory.Item)
						{
							if (thing.def.EverHaulable)
							{
								result = HaulAIUtility.HaulAsideJobFor(pawn, thing);
								goto IL_019d;
							}
							Log.ErrorOnce("Never haulable " + thing + " blocking " + t + " at " + t.Position, 6429262);
						}
						result = null;
					}
					else if (!GenConstruct.CanConstruct(blueprint, pawn, forced))
					{
						result = null;
					}
					else
					{
						Job job = this.DeconstructExistingBuildingJob(pawn, blueprint);
						if (job != null)
						{
							result = job;
						}
						else
						{
							Job job2 = base.RemoveExistingFloorJob(pawn, blueprint);
							if (job2 != null)
							{
								result = job2;
							}
							else
							{
								Job job3 = base.ResourceDeliverJobFor(pawn, blueprint, true);
								if (job3 != null)
								{
									result = job3;
								}
								else
								{
									Job job4 = this.NoCostFrameMakeJobFor(pawn, blueprint);
									result = ((job4 == null) ? null : job4);
								}
							}
						}
					}
				}
			}
			goto IL_019d;
			IL_019d:
			return result;
		}

		private Job DeconstructExistingBuildingJob(Pawn pawn, Blueprint blue)
		{
			Building firstBlockingBuilding = WorkGiver_ConstructDeliverResourcesToBlueprints.GetFirstBlockingBuilding(blue, pawn);
			Job result;
			if (firstBlockingBuilding == null || !pawn.CanReserve((Thing)firstBlockingBuilding, 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.Deconstruct, (Thing)firstBlockingBuilding);
				job.ignoreDesignations = true;
				result = job;
			}
			return result;
		}

		private Job NoCostFrameMakeJobFor(Pawn pawn, IConstructible c)
		{
			Job result;
			if (c is Blueprint_Install)
			{
				result = null;
			}
			else if (c is Blueprint && c.MaterialsNeeded().Count == 0)
			{
				Job job = new Job(JobDefOf.PlaceNoCostFrame);
				job.targetA = (Thing)c;
				result = job;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static Building GetFirstBlockingBuilding(Blueprint blue, Pawn pawn)
		{
			Thing thing = GenConstruct.MiniToInstallOrBuildingToReinstall(blue);
			CellRect cellRect = blue.OccupiedRect();
			int num = cellRect.minZ;
			Building result;
			while (true)
			{
				List<Thing> thingList;
				int j;
				if (num <= cellRect.maxZ)
				{
					for (int i = cellRect.minX; i <= cellRect.maxX; i++)
					{
						IntVec3 c = new IntVec3(i, 0, num);
						thingList = c.GetThingList(pawn.Map);
						for (j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def.category == ThingCategory.Building && thingList[j] != thing && GenSpawn.SpawningWipes(blue.def.entityDefToBuild, thingList[j].def))
								goto IL_0098;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
				IL_0098:
				result = (Building)thingList[j];
				break;
			}
			return result;
		}
	}
}

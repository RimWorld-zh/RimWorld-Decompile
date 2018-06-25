using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		public WorkGiver_ConstructDeliverResourcesToBlueprints()
		{
		}

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
				else if (GenConstruct.FirstBlockingThing(blueprint, pawn) != null)
				{
					result = GenConstruct.HandleBlockingThingJob(blueprint, pawn, forced);
				}
				else
				{
					bool flag = this.def.workType == WorkTypeDefOf.Construction;
					if (!GenConstruct.CanConstruct(blueprint, pawn, flag, forced))
					{
						result = null;
					}
					else if (!flag && WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
					{
						result = null;
					}
					else
					{
						Job job = base.RemoveExistingFloorJob(pawn, blueprint);
						if (job != null)
						{
							result = job;
						}
						else
						{
							Job job2 = base.ResourceDeliverJobFor(pawn, blueprint, true);
							if (job2 != null)
							{
								result = job2;
							}
							else
							{
								Job job3 = this.NoCostFrameMakeJobFor(pawn, blueprint);
								if (job3 != null)
								{
									result = job3;
								}
								else
								{
									result = null;
								}
							}
						}
					}
				}
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
				result = new Job(JobDefOf.PlaceNoCostFrame)
				{
					targetA = (Thing)c
				};
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}

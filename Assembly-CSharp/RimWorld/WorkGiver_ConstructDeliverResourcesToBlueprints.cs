using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200012B RID: 299
	public class WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x000414D0 File Offset: 0x0003F8D0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000414EC File Offset: 0x0003F8EC
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

		// Token: 0x0600062C RID: 1580 RVA: 0x000415D4 File Offset: 0x0003F9D4
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

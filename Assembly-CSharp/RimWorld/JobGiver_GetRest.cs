using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000E2 RID: 226
	public class JobGiver_GetRest : ThinkNode_JobGiver
	{
		// Token: 0x040002B8 RID: 696
		private RestCategory minCategory = RestCategory.Rested;

		// Token: 0x060004E9 RID: 1257 RVA: 0x00036A48 File Offset: 0x00034E48
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetRest jobGiver_GetRest = (JobGiver_GetRest)base.DeepCopy(resolve);
			jobGiver_GetRest.minCategory = this.minCategory;
			return jobGiver_GetRest;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00036A78 File Offset: 0x00034E78
		public override float GetPriority(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			float result;
			if (rest == null)
			{
				result = 0f;
			}
			else if (rest.CurCategory < this.minCategory)
			{
				result = 0f;
			}
			else if (Find.TickManager.TicksGame < pawn.mindState.canSleepTick)
			{
				result = 0f;
			}
			else
			{
				Lord lord = pawn.GetLord();
				if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
				{
					result = 0f;
				}
				else
				{
					TimeAssignmentDef timeAssignmentDef;
					if (pawn.RaceProps.Humanlike)
					{
						timeAssignmentDef = ((pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything);
					}
					else
					{
						int num = GenLocalDate.HourOfDay(pawn);
						if (num < 7 || num > 21)
						{
							timeAssignmentDef = TimeAssignmentDefOf.Sleep;
						}
						else
						{
							timeAssignmentDef = TimeAssignmentDefOf.Anything;
						}
					}
					float curLevel = rest.CurLevel;
					if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
					{
						if (curLevel < 0.3f)
						{
							result = 8f;
						}
						else
						{
							result = 0f;
						}
					}
					else if (timeAssignmentDef == TimeAssignmentDefOf.Work)
					{
						result = 0f;
					}
					else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
					{
						if (curLevel < 0.3f)
						{
							result = 8f;
						}
						else
						{
							result = 0f;
						}
					}
					else
					{
						if (timeAssignmentDef != TimeAssignmentDefOf.Sleep)
						{
							throw new NotImplementedException();
						}
						if (curLevel < RestUtility.FallAsleepMaxLevel(pawn))
						{
							result = 8f;
						}
						else
						{
							result = 0f;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00036C24 File Offset: 0x00035024
		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			Job result;
			if (rest == null || rest.CurCategory < this.minCategory)
			{
				result = null;
			}
			else if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				result = null;
			}
			else
			{
				Lord lord = pawn.GetLord();
				Building_Bed building_Bed;
				if ((lord != null && lord.CurLordToil != null && !lord.CurLordToil.AllowRestingInBed) || pawn.IsWildMan())
				{
					building_Bed = null;
				}
				else
				{
					building_Bed = RestUtility.FindBedFor(pawn);
				}
				if (building_Bed != null)
				{
					Job job = new Job(JobDefOf.LayDown, building_Bed);
					result = job;
				}
				else
				{
					result = new Job(JobDefOf.LayDown, this.FindGroundSleepSpotFor(pawn));
				}
			}
			return result;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00036CF0 File Offset: 0x000350F0
		private IntVec3 FindGroundSleepSpotFor(Pawn pawn)
		{
			Map map = pawn.Map;
			for (int i = 0; i < 2; i++)
			{
				int radius = (i != 0) ? 12 : 4;
				IntVec3 result;
				if (CellFinder.TryRandomClosewalkCellNear(pawn.Position, map, radius, out result, (IntVec3 x) => !x.IsForbidden(pawn) && !x.GetTerrain(map).avoidWander))
				{
					return result;
				}
			}
			return CellFinder.RandomClosewalkCellNearNotForbidden(pawn.Position, map, 4, pawn);
		}
	}
}

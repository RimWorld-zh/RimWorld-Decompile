using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_GetRest : ThinkNode_JobGiver
	{
		private RestCategory minCategory;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetRest jobGiver_GetRest = (JobGiver_GetRest)base.DeepCopy(resolve);
			jobGiver_GetRest.minCategory = this.minCategory;
			return jobGiver_GetRest;
		}

		public override float GetPriority(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			if (rest == null)
			{
				return 0f;
			}
			if ((int)rest.CurCategory < (int)this.minCategory)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < pawn.mindState.canSleepTick)
			{
				return 0f;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
			{
				return 0f;
			}
			TimeAssignmentDef timeAssignmentDef;
			if (pawn.RaceProps.Humanlike)
			{
				timeAssignmentDef = ((pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything);
			}
			else
			{
				int num = GenLocalDate.HourOfDay(pawn);
				timeAssignmentDef = ((num >= 7 && num <= 21) ? TimeAssignmentDefOf.Anything : TimeAssignmentDefOf.Sleep);
			}
			float curLevel = rest.CurLevel;
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				if (curLevel < 0.30000001192092896)
				{
					return 8f;
				}
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Work)
			{
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				if (curLevel < 0.30000001192092896)
				{
					return 8f;
				}
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				if (curLevel < RestUtility.FallAsleepMaxLevel(pawn))
				{
					return 8f;
				}
				return 0f;
			}
			throw new NotImplementedException();
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			Building_Bed building_Bed;
			if (rest != null && (int)rest.CurCategory >= (int)this.minCategory)
			{
				if (RestUtility.DisturbancePreventsLyingDown(pawn))
				{
					return null;
				}
				Lord lord = pawn.GetLord();
				if (lord != null && lord.CurLordToil != null && !lord.CurLordToil.AllowRestingInBed)
				{
					goto IL_0065;
				}
				if (pawn.IsWildMan())
					goto IL_0065;
				building_Bed = RestUtility.FindBedFor(pawn);
				goto IL_0073;
			}
			return null;
			IL_0073:
			if (building_Bed != null)
			{
				return new Job(JobDefOf.LayDown, building_Bed);
			}
			return new Job(JobDefOf.LayDown, this.FindGroundSleepSpotFor(pawn));
			IL_0065:
			building_Bed = null;
			goto IL_0073;
		}

		private IntVec3 FindGroundSleepSpotFor(Pawn pawn)
		{
			Map map = pawn.Map;
			for (int i = 0; i < 2; i++)
			{
				int radius = (i != 0) ? 12 : 4;
				IntVec3 result = default(IntVec3);
				if (CellFinder.TryRandomClosewalkCellNear(pawn.Position, map, radius, out result, (Predicate<IntVec3>)((IntVec3 x) => !x.IsForbidden(pawn) && !x.GetTerrain(map).avoidWander)))
				{
					return result;
				}
			}
			return CellFinder.RandomClosewalkCellNearNotForbidden(pawn.Position, map, 4, pawn);
		}
	}
}

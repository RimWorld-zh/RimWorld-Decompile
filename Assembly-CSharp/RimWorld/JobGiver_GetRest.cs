using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_GetRest : ThinkNode_JobGiver
	{
		private RestCategory minCategory = RestCategory.Rested;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetRest jobGiver_GetRest = (JobGiver_GetRest)base.DeepCopy(resolve);
			jobGiver_GetRest.minCategory = this.minCategory;
			return jobGiver_GetRest;
		}

		public override float GetPriority(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			float result;
			if (rest == null)
			{
				result = 0f;
				goto IL_019b;
			}
			if ((int)rest.CurCategory < (int)this.minCategory)
			{
				result = 0f;
				goto IL_019b;
			}
			if (Find.TickManager.TicksGame < pawn.mindState.canSleepTick)
			{
				result = 0f;
				goto IL_019b;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
			{
				result = 0f;
				goto IL_019b;
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
				result = (float)((!(curLevel < 0.30000001192092896)) ? 0.0 : 8.0);
				goto IL_019b;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Work)
			{
				result = 0f;
				goto IL_019b;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				result = (float)((!(curLevel < 0.30000001192092896)) ? 0.0 : 8.0);
				goto IL_019b;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				result = (float)((!(curLevel < RestUtility.FallAsleepMaxLevel(pawn))) ? 0.0 : 8.0);
				goto IL_019b;
			}
			throw new NotImplementedException();
			IL_019b:
			return result;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			Job result;
			Building_Bed building_Bed;
			if (rest != null && (int)rest.CurCategory >= (int)this.minCategory)
			{
				if (RestUtility.DisturbancePreventsLyingDown(pawn))
				{
					result = null;
					goto IL_00bc;
				}
				Lord lord = pawn.GetLord();
				if (lord != null && lord.CurLordToil != null && !lord.CurLordToil.AllowRestingInBed)
				{
					goto IL_0070;
				}
				if (pawn.IsWildMan())
					goto IL_0070;
				building_Bed = RestUtility.FindBedFor(pawn);
				goto IL_007e;
			}
			result = null;
			goto IL_00bc;
			IL_007e:
			if (building_Bed != null)
			{
				Job job = result = new Job(JobDefOf.LayDown, (Thing)building_Bed);
			}
			else
			{
				result = new Job(JobDefOf.LayDown, this.FindGroundSleepSpotFor(pawn));
			}
			goto IL_00bc;
			IL_00bc:
			return result;
			IL_0070:
			building_Bed = null;
			goto IL_007e;
		}

		private IntVec3 FindGroundSleepSpotFor(Pawn pawn)
		{
			Map map = pawn.Map;
			int num = 0;
			IntVec3 result;
			while (true)
			{
				if (num < 2)
				{
					int radius = (num != 0) ? 12 : 4;
					IntVec3 intVec = default(IntVec3);
					if (CellFinder.TryRandomClosewalkCellNear(pawn.Position, map, radius, out intVec, (Predicate<IntVec3>)((IntVec3 x) => !x.IsForbidden(pawn) && !x.GetTerrain(map).avoidWander)))
					{
						result = intVec;
						break;
					}
					num++;
					continue;
				}
				result = CellFinder.RandomClosewalkCellNearNotForbidden(pawn.Position, map, 4, pawn);
				break;
			}
			return result;
		}
	}
}

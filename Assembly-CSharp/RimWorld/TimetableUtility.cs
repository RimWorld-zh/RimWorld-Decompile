using System;
using Verse;

namespace RimWorld
{
	public static class TimetableUtility
	{
		public static TimeAssignmentDef GetTimeAssignment(this Pawn pawn)
		{
			TimeAssignmentDef result;
			if (pawn.timetable == null)
			{
				result = TimeAssignmentDefOf.Anything;
			}
			else
			{
				result = pawn.timetable.CurrentAssignment;
			}
			return result;
		}
	}
}

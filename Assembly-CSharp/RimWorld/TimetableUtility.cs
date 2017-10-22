using Verse;

namespace RimWorld
{
	public static class TimetableUtility
	{
		public static TimeAssignmentDef GetTimeAssignment(this Pawn pawn)
		{
			return (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
		}
	}
}

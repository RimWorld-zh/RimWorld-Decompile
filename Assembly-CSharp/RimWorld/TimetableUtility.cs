using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000519 RID: 1305
	public static class TimetableUtility
	{
		// Token: 0x060017B6 RID: 6070 RVA: 0x000CF294 File Offset: 0x000CD694
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

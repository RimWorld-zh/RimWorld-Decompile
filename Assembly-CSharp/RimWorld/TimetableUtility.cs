using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051B RID: 1307
	public static class TimetableUtility
	{
		// Token: 0x060017BB RID: 6075 RVA: 0x000CF14C File Offset: 0x000CD54C
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

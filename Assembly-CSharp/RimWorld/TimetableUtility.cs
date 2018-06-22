using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000517 RID: 1303
	public static class TimetableUtility
	{
		// Token: 0x060017B2 RID: 6066 RVA: 0x000CF144 File Offset: 0x000CD544
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

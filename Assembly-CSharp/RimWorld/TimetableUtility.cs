using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051B RID: 1307
	public static class TimetableUtility
	{
		// Token: 0x060017BA RID: 6074 RVA: 0x000CF0F8 File Offset: 0x000CD4F8
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

using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_Priority_GetJoy : ThinkNode_Priority
	{
		private const int GameStartNoJoyTicks = 5000;

		public override float GetPriority(Pawn pawn)
		{
			float result;
			if (pawn.needs.joy == null)
			{
				result = 0f;
				goto IL_0126;
			}
			if (Find.TickManager.TicksGame < 5000)
			{
				result = 0f;
				goto IL_0126;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn))
			{
				result = 0f;
				goto IL_0126;
			}
			float curLevel = pawn.needs.joy.CurLevel;
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
			if (!timeAssignmentDef.allowJoy)
			{
				result = 0f;
				goto IL_0126;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				result = (float)((!(curLevel < 0.34999999403953552)) ? 0.0 : 6.0);
				goto IL_0126;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				result = (float)((!(curLevel < 0.949999988079071)) ? 0.0 : 7.0);
				goto IL_0126;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				result = (float)((!(curLevel < 0.949999988079071)) ? 0.0 : 2.0);
				goto IL_0126;
			}
			throw new NotImplementedException();
			IL_0126:
			return result;
		}
	}
}

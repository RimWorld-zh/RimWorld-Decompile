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
			if (pawn.needs.joy == null)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < 5000)
			{
				return 0f;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn))
			{
				return 0f;
			}
			float curLevel = pawn.needs.joy.CurLevel;
			TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
			if (!timeAssignmentDef.allowJoy)
			{
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				if (curLevel < 0.34999999403953552)
				{
					return 6f;
				}
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
			{
				if (curLevel < 0.949999988079071)
				{
					return 7f;
				}
				return 0f;
			}
			if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
			{
				if (curLevel < 0.949999988079071)
				{
					return 2f;
				}
				return 0f;
			}
			throw new NotImplementedException();
		}
	}
}

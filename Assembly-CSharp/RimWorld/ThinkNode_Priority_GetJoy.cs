using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001B8 RID: 440
	public class ThinkNode_Priority_GetJoy : ThinkNode_Priority
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x00055C18 File Offset: 0x00054018
		public override float GetPriority(Pawn pawn)
		{
			float result;
			if (pawn.needs.joy == null)
			{
				result = 0f;
			}
			else if (Find.TickManager.TicksGame < 5000)
			{
				result = 0f;
			}
			else if (JoyUtility.LordPreventsGettingJoy(pawn))
			{
				result = 0f;
			}
			else
			{
				float curLevel = pawn.needs.joy.CurLevel;
				TimeAssignmentDef timeAssignmentDef = (pawn.timetable != null) ? pawn.timetable.CurrentAssignment : TimeAssignmentDefOf.Anything;
				if (!timeAssignmentDef.allowJoy)
				{
					result = 0f;
				}
				else if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
				{
					if (curLevel < 0.35f)
					{
						result = 6f;
					}
					else
					{
						result = 0f;
					}
				}
				else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
				{
					if (curLevel < 0.95f)
					{
						result = 7f;
					}
					else
					{
						result = 0f;
					}
				}
				else
				{
					if (timeAssignmentDef != TimeAssignmentDefOf.Sleep)
					{
						throw new NotImplementedException();
					}
					if (curLevel < 0.95f)
					{
						result = 2f;
					}
					else
					{
						result = 0f;
					}
				}
			}
			return result;
		}

		// Token: 0x040003DC RID: 988
		private const int GameStartNoJoyTicks = 5000;
	}
}

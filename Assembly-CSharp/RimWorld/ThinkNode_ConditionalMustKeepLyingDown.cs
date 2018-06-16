using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DF RID: 479
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06000979 RID: 2425 RVA: 0x00056838 File Offset: 0x00054C38
		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (pawn.CurJob == null || !pawn.GetPosture().Laying())
			{
				result = false;
			}
			else
			{
				if (!pawn.Downed)
				{
					if (RestUtility.DisturbancePreventsLyingDown(pawn))
					{
						return false;
					}
					if (!pawn.CurJob.restUntilHealed || !HealthAIUtility.ShouldSeekMedicalRest(pawn))
					{
						if (!pawn.jobs.curDriver.asleep)
						{
							return false;
						}
						if (!pawn.CurJob.playerForced)
						{
							if (RestUtility.TimetablePreventsLayDown(pawn))
							{
								return false;
							}
						}
					}
				}
				result = true;
			}
			return result;
		}
	}
}

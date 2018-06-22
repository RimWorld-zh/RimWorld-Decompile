using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DF RID: 479
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x0005684C File Offset: 0x00054C4C
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

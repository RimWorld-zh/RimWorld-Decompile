using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalMustKeepLyingDown()
		{
		}

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

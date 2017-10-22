using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (pawn.CurJob == null || pawn.jobs.curDriver.layingDown == LayingDownState.NotLaying)
			{
				result = false;
			}
			else
			{
				if (!pawn.Downed)
				{
					if (RestUtility.DisturbancePreventsLyingDown(pawn))
					{
						result = false;
						goto IL_00ab;
					}
					if (!pawn.CurJob.restUntilHealed || !HealthAIUtility.ShouldSeekMedicalRest(pawn))
					{
						if (!pawn.jobs.curDriver.asleep)
						{
							result = false;
							goto IL_00ab;
						}
						if (!pawn.CurJob.playerForced && RestUtility.TimetablePreventsLayDown(pawn))
						{
							result = false;
							goto IL_00ab;
						}
					}
				}
				result = true;
			}
			goto IL_00ab;
			IL_00ab:
			return result;
		}
	}
}

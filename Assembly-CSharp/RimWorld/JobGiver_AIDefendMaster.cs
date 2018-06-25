using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000B6 RID: 182
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		// Token: 0x04000286 RID: 646
		private const float RadiusUnreleased = 5f;

		// Token: 0x06000455 RID: 1109 RVA: 0x00032B64 File Offset: 0x00030F64
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00032B84 File Offset: 0x00030F84
		protected override float GetFlagRadius(Pawn pawn)
		{
			float result;
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				result = 50f;
			}
			else
			{
				result = 5f;
			}
			return result;
		}
	}
}

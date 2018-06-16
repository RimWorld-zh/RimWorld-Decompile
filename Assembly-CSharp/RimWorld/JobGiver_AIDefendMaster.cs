using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000B6 RID: 182
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x00032B6C File Offset: 0x00030F6C
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00032B8C File Offset: 0x00030F8C
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

		// Token: 0x04000285 RID: 645
		private const float RadiusUnreleased = 5f;
	}
}

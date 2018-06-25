using System;
using Verse;

namespace RimWorld
{
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		private const float RadiusUnreleased = 5f;

		public JobGiver_AIDefendMaster()
		{
		}

		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

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

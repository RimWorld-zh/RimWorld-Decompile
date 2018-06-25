using System;
using Verse;

namespace RimWorld
{
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		public const float RadiusUnreleased = 3f;

		public const float RadiusReleased = 50f;

		public JobGiver_AIFollowMaster()
		{
		}

		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		protected override Pawn GetFollowee(Pawn pawn)
		{
			Pawn result;
			if (pawn.playerSettings == null)
			{
				result = null;
			}
			else
			{
				result = pawn.playerSettings.Master;
			}
			return result;
		}

		protected override float GetRadius(Pawn pawn)
		{
			float result;
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				result = 50f;
			}
			else
			{
				result = 3f;
			}
			return result;
		}
	}
}

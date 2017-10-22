using Verse;

namespace RimWorld
{
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		public const float RadiusUnreleased = 3f;

		public const float RadiusReleased = 50f;

		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (pawn.playerSettings != null) ? pawn.playerSettings.master : null;
		}

		protected override float GetRadius(Pawn pawn)
		{
			return (float)((!pawn.playerSettings.master.playerSettings.animalsReleased || !pawn.training.IsCompleted(TrainableDefOf.Release)) ? 3.0 : 50.0);
		}
	}
}

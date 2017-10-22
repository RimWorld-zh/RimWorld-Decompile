using RimWorld;

namespace Verse.AI
{
	public class MentalState_WanderOwnRoom : MentalState
	{
		public IntVec3 target;

		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			if (base.pawn.ownership.OwnedBed != null)
			{
				this.target = base.pawn.ownership.OwnedBed.Position;
			}
			else
			{
				this.target = base.pawn.Position;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

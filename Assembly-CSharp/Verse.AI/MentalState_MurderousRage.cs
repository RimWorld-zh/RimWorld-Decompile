using RimWorld;

namespace Verse.AI
{
	public class MentalState_MurderousRage : MentalState
	{
		public Pawn target;

		private const int NoLongerValidTargetCheckInterval = 120;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.target = MurderousRageMentalStateUtility.FindPawn(base.pawn);
		}

		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (base.pawn.IsHashIntervalTick(120))
			{
				if (this.target != null && this.target.Spawned && base.pawn.CanReach((Thing)this.target, PathEndMode.Touch, Danger.Deadly, true, TraverseMode.ByPawn))
					return;
				base.RecoverFromState();
			}
		}

		public override string GetBeginLetterText()
		{
			string result;
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				result = "";
			}
			else
			{
				result = string.Format(base.def.beginLetter, base.pawn.Label, this.target.LabelShort).AdjustedFor(base.pawn).CapitalizeFirst();
			}
			return result;
		}
	}
}

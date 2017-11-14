using RimWorld;

namespace Verse.AI
{
	public class MentalState_SocialFighting : MentalState
	{
		public Pawn otherPawn;

		private bool ShouldStop
		{
			get
			{
				if (this.otherPawn.Spawned && !this.otherPawn.Dead && !this.otherPawn.Downed)
				{
					if (!this.IsOtherPawnSocialFightingWithMe)
					{
						return true;
					}
					return false;
				}
				return true;
			}
		}

		private bool IsOtherPawnSocialFightingWithMe
		{
			get
			{
				if (!this.otherPawn.InMentalState)
				{
					return false;
				}
				MentalState_SocialFighting mentalState_SocialFighting = this.otherPawn.MentalState as MentalState_SocialFighting;
				if (mentalState_SocialFighting == null)
				{
					return false;
				}
				if (mentalState_SocialFighting.otherPawn != base.pawn)
				{
					return false;
				}
				return true;
			}
		}

		public override void MentalStateTick()
		{
			if (this.ShouldStop)
			{
				base.RecoverFromState();
			}
			else
			{
				base.MentalStateTick();
			}
		}

		public override void PostEnd()
		{
			base.PostEnd();
			base.pawn.jobs.StopAll(false);
			base.pawn.mindState.meleeThreat = null;
			if (this.IsOtherPawnSocialFightingWithMe)
			{
				this.otherPawn.MentalState.RecoverFromState();
			}
			if ((PawnUtility.ShouldSendNotificationAbout(base.pawn) || PawnUtility.ShouldSendNotificationAbout(this.otherPawn)) && base.pawn.thingIDNumber < this.otherPawn.thingIDNumber)
			{
				Messages.Message("MessageNoLongerSocialFighting".Translate(base.pawn.NameStringShort, this.otherPawn.LabelShort), base.pawn, MessageTypeDefOf.SituationResolved);
			}
			if (!base.pawn.Dead && base.pawn.needs.mood != null && !this.otherPawn.Dead)
			{
				ThoughtDef def = (!(Rand.Value < 0.5)) ? ThoughtDefOf.HadCatharticFight : ThoughtDefOf.HadAngeringFight;
				base.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

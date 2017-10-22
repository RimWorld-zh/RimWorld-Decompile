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
				return (byte)((!this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed) ? 1 : ((!this.IsOtherPawnSocialFightingWithMe) ? 1 : 0)) != 0;
			}
		}

		private bool IsOtherPawnSocialFightingWithMe
		{
			get
			{
				bool result;
				if (!this.otherPawn.InMentalState)
				{
					result = false;
				}
				else
				{
					MentalState_SocialFighting mentalState_SocialFighting = this.otherPawn.MentalState as MentalState_SocialFighting;
					result = ((byte)((mentalState_SocialFighting != null) ? ((mentalState_SocialFighting.otherPawn == base.pawn) ? 1 : 0) : 0) != 0);
				}
				return result;
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
				Messages.Message("MessageNoLongerSocialFighting".Translate(base.pawn.NameStringShort, this.otherPawn.LabelShort), (Thing)base.pawn, MessageTypeDefOf.SituationResolved);
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

using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A81 RID: 2689
	public class MentalState_SocialFighting : MentalState
	{
		// Token: 0x04002579 RID: 9593
		public Pawn otherPawn;

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x001F8208 File Offset: 0x001F6608
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003BB1 RID: 15281 RVA: 0x001F8268 File Offset: 0x001F6668
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
					result = (mentalState_SocialFighting != null && mentalState_SocialFighting.otherPawn == this.pawn);
				}
				return result;
			}
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x001F82CB File Offset: 0x001F66CB
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

		// Token: 0x06003BB3 RID: 15283 RVA: 0x001F82EC File Offset: 0x001F66EC
		public override void PostEnd()
		{
			base.PostEnd();
			this.pawn.jobs.StopAll(false);
			this.pawn.mindState.meleeThreat = null;
			if (this.IsOtherPawnSocialFightingWithMe)
			{
				this.otherPawn.MentalState.RecoverFromState();
			}
			if ((PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.otherPawn)) && this.pawn.thingIDNumber < this.otherPawn.thingIDNumber)
			{
				Messages.Message("MessageNoLongerSocialFighting".Translate(new object[]
				{
					this.pawn.LabelShort,
					this.otherPawn.LabelShort
				}), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
			if (!this.pawn.Dead && this.pawn.needs.mood != null && !this.otherPawn.Dead)
			{
				ThoughtDef def;
				if (Rand.Value < 0.5f)
				{
					def = ThoughtDefOf.HadAngeringFight;
				}
				else
				{
					def = ThoughtDefOf.HadCatharticFight;
				}
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn);
			}
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x001F8439 File Offset: 0x001F6839
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x001F8454 File Offset: 0x001F6854
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

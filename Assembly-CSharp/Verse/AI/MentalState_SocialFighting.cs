using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A83 RID: 2691
	public class MentalState_SocialFighting : MentalState
	{
		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003BAF RID: 15279 RVA: 0x001F7CF4 File Offset: 0x001F60F4
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x001F7D54 File Offset: 0x001F6154
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

		// Token: 0x06003BB1 RID: 15281 RVA: 0x001F7DB7 File Offset: 0x001F61B7
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

		// Token: 0x06003BB2 RID: 15282 RVA: 0x001F7DD8 File Offset: 0x001F61D8
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

		// Token: 0x06003BB3 RID: 15283 RVA: 0x001F7F25 File Offset: 0x001F6325
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x001F7F40 File Offset: 0x001F6340
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x0400257D RID: 9597
		public Pawn otherPawn;
	}
}

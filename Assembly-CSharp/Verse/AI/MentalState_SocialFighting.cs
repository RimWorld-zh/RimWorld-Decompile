using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A82 RID: 2690
	public class MentalState_SocialFighting : MentalState
	{
		// Token: 0x04002589 RID: 9609
		public Pawn otherPawn;

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003BB1 RID: 15281 RVA: 0x001F8534 File Offset: 0x001F6934
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003BB2 RID: 15282 RVA: 0x001F8594 File Offset: 0x001F6994
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

		// Token: 0x06003BB3 RID: 15283 RVA: 0x001F85F7 File Offset: 0x001F69F7
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

		// Token: 0x06003BB4 RID: 15284 RVA: 0x001F8618 File Offset: 0x001F6A18
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

		// Token: 0x06003BB5 RID: 15285 RVA: 0x001F8765 File Offset: 0x001F6B65
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x001F8780 File Offset: 0x001F6B80
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

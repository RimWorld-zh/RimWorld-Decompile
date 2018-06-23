using System;

namespace Verse
{
	// Token: 0x02000D5D RID: 3421
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x0400332B RID: 13099
		public int ticksLeft = 0;

		// Token: 0x0400332C RID: 13100
		public Verb verb;

		// Token: 0x0400332D RID: 13101
		public LocalTargetInfo focusTarg;

		// Token: 0x0400332E RID: 13102
		public bool neverAimWeapon = false;

		// Token: 0x0400332F RID: 13103
		protected float pieSizeFactor = 1f;

		// Token: 0x06004CBA RID: 19642 RVA: 0x0027FED5 File Offset: 0x0027E2D5
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x0027FEFD File Offset: 0x0027E2FD
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x0027FF34 File Offset: 0x0027E334
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06004CBD RID: 19645 RVA: 0x0027FF48 File Offset: 0x0027E348
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0027FF60 File Offset: 0x0027E360
		private void SetPieSizeFactor()
		{
			if (this.ticksLeft < 300)
			{
				this.pieSizeFactor = 1f;
			}
			else if (this.ticksLeft < 450)
			{
				this.pieSizeFactor = 0.75f;
			}
			else
			{
				this.pieSizeFactor = 0.5f;
			}
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0027FFBC File Offset: 0x0027E3BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.focusTarg, "focusTarg");
			Scribe_Values.Look<bool>(ref this.neverAimWeapon, "neverAimWeapon", false, false);
			Scribe_References.Look<Verb>(ref this.verb, "verb", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetPieSizeFactor();
			}
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x00280026 File Offset: 0x0027E426
		public override void StanceTick()
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x00280049 File Offset: 0x0027E449
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}
	}
}

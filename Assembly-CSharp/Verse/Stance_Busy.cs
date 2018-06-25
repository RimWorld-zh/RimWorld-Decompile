using System;

namespace Verse
{
	// Token: 0x02000D5F RID: 3423
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

		// Token: 0x06004CBE RID: 19646 RVA: 0x00280001 File Offset: 0x0027E401
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x00280029 File Offset: 0x0027E429
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x00280060 File Offset: 0x0027E460
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x00280074 File Offset: 0x0027E474
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x0028008C File Offset: 0x0027E48C
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

		// Token: 0x06004CC3 RID: 19651 RVA: 0x002800E8 File Offset: 0x0027E4E8
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

		// Token: 0x06004CC4 RID: 19652 RVA: 0x00280152 File Offset: 0x0027E552
		public override void StanceTick()
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x00280175 File Offset: 0x0027E575
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}
	}
}

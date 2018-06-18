using System;

namespace Verse
{
	// Token: 0x02000D60 RID: 3424
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x06004CA5 RID: 19621 RVA: 0x0027E925 File Offset: 0x0027CD25
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x0027E94D File Offset: 0x0027CD4D
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x0027E984 File Offset: 0x0027CD84
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x06004CA8 RID: 19624 RVA: 0x0027E998 File Offset: 0x0027CD98
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x0027E9B0 File Offset: 0x0027CDB0
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

		// Token: 0x06004CAA RID: 19626 RVA: 0x0027EA0C File Offset: 0x0027CE0C
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

		// Token: 0x06004CAB RID: 19627 RVA: 0x0027EA76 File Offset: 0x0027CE76
		public override void StanceTick()
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x0027EA99 File Offset: 0x0027CE99
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x04003320 RID: 13088
		public int ticksLeft = 0;

		// Token: 0x04003321 RID: 13089
		public Verb verb;

		// Token: 0x04003322 RID: 13090
		public LocalTargetInfo focusTarg;

		// Token: 0x04003323 RID: 13091
		public bool neverAimWeapon = false;

		// Token: 0x04003324 RID: 13092
		protected float pieSizeFactor = 1f;
	}
}

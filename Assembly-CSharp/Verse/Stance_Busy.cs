using System;

namespace Verse
{
	// Token: 0x02000D61 RID: 3425
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x06004CA7 RID: 19623 RVA: 0x0027E945 File Offset: 0x0027CD45
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x0027E96D File Offset: 0x0027CD6D
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x0027E9A4 File Offset: 0x0027CDA4
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06004CAA RID: 19626 RVA: 0x0027E9B8 File Offset: 0x0027CDB8
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x0027E9D0 File Offset: 0x0027CDD0
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

		// Token: 0x06004CAC RID: 19628 RVA: 0x0027EA2C File Offset: 0x0027CE2C
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

		// Token: 0x06004CAD RID: 19629 RVA: 0x0027EA96 File Offset: 0x0027CE96
		public override void StanceTick()
		{
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x0027EAB9 File Offset: 0x0027CEB9
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x04003322 RID: 13090
		public int ticksLeft = 0;

		// Token: 0x04003323 RID: 13091
		public Verb verb;

		// Token: 0x04003324 RID: 13092
		public LocalTargetInfo focusTarg;

		// Token: 0x04003325 RID: 13093
		public bool neverAimWeapon = false;

		// Token: 0x04003326 RID: 13094
		protected float pieSizeFactor = 1f;
	}
}

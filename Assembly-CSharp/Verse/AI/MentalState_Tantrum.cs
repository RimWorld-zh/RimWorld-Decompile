using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A86 RID: 2694
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x06003BC0 RID: 15296 RVA: 0x001F8032 File Offset: 0x001F6432
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x001F8060 File Offset: 0x001F6460
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001F8076 File Offset: 0x001F6476
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hitTargetAtLeastOnce = true;
			}
		}

		// Token: 0x04002580 RID: 9600
		public Thing target;

		// Token: 0x04002581 RID: 9601
		protected bool hitTargetAtLeastOnce;
	}
}

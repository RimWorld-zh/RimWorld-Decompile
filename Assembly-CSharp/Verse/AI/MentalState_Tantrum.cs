using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A82 RID: 2690
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x0400257B RID: 9595
		public Thing target;

		// Token: 0x0400257C RID: 9596
		protected bool hitTargetAtLeastOnce;

		// Token: 0x06003BBB RID: 15291 RVA: 0x001F8346 File Offset: 0x001F6746
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x001F8374 File Offset: 0x001F6774
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x001F838A File Offset: 0x001F678A
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hitTargetAtLeastOnce = true;
			}
		}
	}
}

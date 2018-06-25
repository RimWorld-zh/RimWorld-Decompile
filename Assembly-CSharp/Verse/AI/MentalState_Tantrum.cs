using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A85 RID: 2693
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x0400258C RID: 9612
		public Thing target;

		// Token: 0x0400258D RID: 9613
		protected bool hitTargetAtLeastOnce;

		// Token: 0x06003BC0 RID: 15296 RVA: 0x001F879E File Offset: 0x001F6B9E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x001F87CC File Offset: 0x001F6BCC
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001F87E2 File Offset: 0x001F6BE2
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

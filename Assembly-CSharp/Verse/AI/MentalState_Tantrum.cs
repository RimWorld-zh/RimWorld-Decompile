using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A84 RID: 2692
	public abstract class MentalState_Tantrum : MentalState
	{
		// Token: 0x0400257C RID: 9596
		public Thing target;

		// Token: 0x0400257D RID: 9597
		protected bool hitTargetAtLeastOnce;

		// Token: 0x06003BBF RID: 15295 RVA: 0x001F8472 File Offset: 0x001F6872
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.hitTargetAtLeastOnce, "hitTargetAtLeastOnce", false, false);
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x001F84A0 File Offset: 0x001F68A0
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x001F84B6 File Offset: 0x001F68B6
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

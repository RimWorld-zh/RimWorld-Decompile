using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6F RID: 2671
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x0400256E RID: 9582
		public Pawn target;

		// Token: 0x0400256F RID: 9583
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04002570 RID: 9584
		public int lastInsultTicks = -999999;

		// Token: 0x06003B50 RID: 15184 RVA: 0x001F7522 File Offset: 0x001F5922
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x001F7560 File Offset: 0x001F5960
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

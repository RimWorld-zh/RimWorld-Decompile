using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6E RID: 2670
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x0400255E RID: 9566
		public Pawn target;

		// Token: 0x0400255F RID: 9567
		public bool insultedTargetAtLeastOnce;

		// Token: 0x04002560 RID: 9568
		public int lastInsultTicks = -999999;

		// Token: 0x06003B4F RID: 15183 RVA: 0x001F71F6 File Offset: 0x001F55F6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x001F7234 File Offset: 0x001F5634
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6C RID: 2668
	public abstract class MentalState_InsultingSpree : MentalState
	{
		// Token: 0x0400255D RID: 9565
		public Pawn target;

		// Token: 0x0400255E RID: 9566
		public bool insultedTargetAtLeastOnce;

		// Token: 0x0400255F RID: 9567
		public int lastInsultTicks = -999999;

		// Token: 0x06003B4B RID: 15179 RVA: 0x001F70CA File Offset: 0x001F54CA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Values.Look<bool>(ref this.insultedTargetAtLeastOnce, "insultedTargetAtLeastOnce", false, false);
			Scribe_Values.Look<int>(ref this.lastInsultTicks, "lastInsultTicks", 0, false);
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x001F7108 File Offset: 0x001F5508
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

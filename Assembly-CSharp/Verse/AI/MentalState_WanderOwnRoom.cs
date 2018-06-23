using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7A RID: 2682
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x04002577 RID: 9591
		public IntVec3 target;

		// Token: 0x06003BA0 RID: 15264 RVA: 0x001F7FB0 File Offset: 0x001F63B0
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			if (this.pawn.ownership.OwnedBed != null)
			{
				this.target = this.pawn.ownership.OwnedBed.Position;
			}
			else
			{
				this.target = this.pawn.Position;
			}
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001F800C File Offset: 0x001F640C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001F803C File Offset: 0x001F643C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

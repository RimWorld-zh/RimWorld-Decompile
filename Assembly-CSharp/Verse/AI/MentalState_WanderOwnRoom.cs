using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7D RID: 2685
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x04002588 RID: 9608
		public IntVec3 target;

		// Token: 0x06003BA5 RID: 15269 RVA: 0x001F8408 File Offset: 0x001F6808
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

		// Token: 0x06003BA6 RID: 15270 RVA: 0x001F8464 File Offset: 0x001F6864
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x001F8494 File Offset: 0x001F6894
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

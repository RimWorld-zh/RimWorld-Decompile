using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7E RID: 2686
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x06003BA5 RID: 15269 RVA: 0x001F7C9C File Offset: 0x001F609C
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

		// Token: 0x06003BA6 RID: 15270 RVA: 0x001F7CF8 File Offset: 0x001F60F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x001F7D28 File Offset: 0x001F6128
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x0400257C RID: 9596
		public IntVec3 target;
	}
}

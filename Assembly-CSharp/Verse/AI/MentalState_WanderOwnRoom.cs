using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7C RID: 2684
	public class MentalState_WanderOwnRoom : MentalState
	{
		// Token: 0x04002578 RID: 9592
		public IntVec3 target;

		// Token: 0x06003BA4 RID: 15268 RVA: 0x001F80DC File Offset: 0x001F64DC
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

		// Token: 0x06003BA5 RID: 15269 RVA: 0x001F8138 File Offset: 0x001F6538
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x001F8168 File Offset: 0x001F6568
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}

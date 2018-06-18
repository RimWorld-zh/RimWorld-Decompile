using System;

namespace Verse.AI
{
	// Token: 0x02000A74 RID: 2676
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x06003B77 RID: 15223 RVA: 0x001F732D File Offset: 0x001F572D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x001F7348 File Offset: 0x001F5748
		public override void MentalStateTick()
		{
			bool flag = false;
			if (this.pawn.IsHashIntervalTick(500) && !CorpseObsessionMentalStateUtility.IsCorpseValid(this.corpse, this.pawn, false))
			{
				this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
				if (this.corpse == null)
				{
					base.RecoverFromState();
					flag = true;
				}
			}
			if (!flag)
			{
				base.MentalStateTick();
			}
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x001F73B8 File Offset: 0x001F57B8
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x001F73D3 File Offset: 0x001F57D3
		public void Notify_CorpseHauled()
		{
			base.RecoverFromState();
		}

		// Token: 0x0400256F RID: 9583
		public Corpse corpse;

		// Token: 0x04002570 RID: 9584
		private const int AnyCorpseStillValidCheckInterval = 500;
	}
}

using System;

namespace Verse.AI
{
	// Token: 0x02000A70 RID: 2672
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x0400256A RID: 9578
		public Corpse corpse;

		// Token: 0x0400256B RID: 9579
		private const int AnyCorpseStillValidCheckInterval = 500;

		// Token: 0x06003B72 RID: 15218 RVA: 0x001F763A File Offset: 0x001F5A3A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x001F7654 File Offset: 0x001F5A54
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

		// Token: 0x06003B74 RID: 15220 RVA: 0x001F76C4 File Offset: 0x001F5AC4
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x001F76DF File Offset: 0x001F5ADF
		public void Notify_CorpseHauled()
		{
			base.RecoverFromState();
		}
	}
}

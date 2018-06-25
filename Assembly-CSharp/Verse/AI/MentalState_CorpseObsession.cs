using System;

namespace Verse.AI
{
	// Token: 0x02000A72 RID: 2674
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x0400256B RID: 9579
		public Corpse corpse;

		// Token: 0x0400256C RID: 9580
		private const int AnyCorpseStillValidCheckInterval = 500;

		// Token: 0x06003B76 RID: 15222 RVA: 0x001F7766 File Offset: 0x001F5B66
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x001F7780 File Offset: 0x001F5B80
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

		// Token: 0x06003B78 RID: 15224 RVA: 0x001F77F0 File Offset: 0x001F5BF0
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x001F780B File Offset: 0x001F5C0B
		public void Notify_CorpseHauled()
		{
			base.RecoverFromState();
		}
	}
}

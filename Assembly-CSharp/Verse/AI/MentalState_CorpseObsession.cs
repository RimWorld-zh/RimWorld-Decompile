using System;

namespace Verse.AI
{
	// Token: 0x02000A73 RID: 2675
	public class MentalState_CorpseObsession : MentalState
	{
		// Token: 0x0400257B RID: 9595
		public Corpse corpse;

		// Token: 0x0400257C RID: 9596
		private const int AnyCorpseStillValidCheckInterval = 500;

		// Token: 0x06003B77 RID: 15223 RVA: 0x001F7A92 File Offset: 0x001F5E92
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x001F7AAC File Offset: 0x001F5EAC
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

		// Token: 0x06003B79 RID: 15225 RVA: 0x001F7B1C File Offset: 0x001F5F1C
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.corpse = CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(this.pawn);
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x001F7B37 File Offset: 0x001F5F37
		public void Notify_CorpseHauled()
		{
			base.RecoverFromState();
		}
	}
}

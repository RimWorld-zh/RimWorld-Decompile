using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A74 RID: 2676
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x0400257D RID: 9597
		private const int NoPrisonerToFreeCheckInterval = 500;

		// Token: 0x06003B7C RID: 15228 RVA: 0x001F7B48 File Offset: 0x001F5F48
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x001F7B80 File Offset: 0x001F5F80
		public void Notify_InducedPrisonerToEscape()
		{
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(this.pawn))
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, false, null, true);
			}
			else if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(this.pawn))
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, null, true);
			}
			else
			{
				base.RecoverFromState();
			}
		}
	}
}

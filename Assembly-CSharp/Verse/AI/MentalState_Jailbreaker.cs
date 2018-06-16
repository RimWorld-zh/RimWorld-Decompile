using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A75 RID: 2677
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x06003B7A RID: 15226 RVA: 0x001F7310 File Offset: 0x001F5710
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x001F7348 File Offset: 0x001F5748
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

		// Token: 0x04002571 RID: 9585
		private const int NoPrisonerToFreeCheckInterval = 500;
	}
}

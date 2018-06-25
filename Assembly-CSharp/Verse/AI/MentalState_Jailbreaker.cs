using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A73 RID: 2675
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x0400256D RID: 9581
		private const int NoPrisonerToFreeCheckInterval = 500;

		// Token: 0x06003B7B RID: 15227 RVA: 0x001F781C File Offset: 0x001F5C1C
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x001F7854 File Offset: 0x001F5C54
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

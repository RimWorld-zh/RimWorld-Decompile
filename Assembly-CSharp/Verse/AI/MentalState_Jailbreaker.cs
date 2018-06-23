using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A71 RID: 2673
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x0400256C RID: 9580
		private const int NoPrisonerToFreeCheckInterval = 500;

		// Token: 0x06003B77 RID: 15223 RVA: 0x001F76F0 File Offset: 0x001F5AF0
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(500) && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x001F7728 File Offset: 0x001F5B28
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

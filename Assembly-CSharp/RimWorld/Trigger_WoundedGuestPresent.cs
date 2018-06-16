using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001B0 RID: 432
	public class Trigger_WoundedGuestPresent : Trigger
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x000539D9 File Offset: 0x00051DD9
		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x000539F0 File Offset: 0x00051DF0
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00053A10 File Offset: 0x00051E10
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 800 == 0)
			{
				TriggerData_PawnCycleInd data = this.Data;
				data.pawnCycleInd++;
				if (data.pawnCycleInd >= lord.ownedPawns.Count)
				{
					data.pawnCycleInd = 0;
				}
				if (lord.ownedPawns.Any<Pawn>())
				{
					Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
					if (pawn.Spawned && !pawn.Downed && !pawn.InMentalState)
					{
						if (KidnapAIUtility.ReachableWoundedGuest(pawn) != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040003C2 RID: 962
		private const int CheckInterval = 800;
	}
}

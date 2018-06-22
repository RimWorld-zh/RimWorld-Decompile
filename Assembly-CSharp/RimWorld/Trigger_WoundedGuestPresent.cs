using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001B0 RID: 432
	public class Trigger_WoundedGuestPresent : Trigger
	{
		// Token: 0x060008E0 RID: 2272 RVA: 0x000539ED File Offset: 0x00051DED
		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00053A04 File Offset: 0x00051E04
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00053A24 File Offset: 0x00051E24
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

		// Token: 0x040003C0 RID: 960
		private const int CheckInterval = 800;
	}
}

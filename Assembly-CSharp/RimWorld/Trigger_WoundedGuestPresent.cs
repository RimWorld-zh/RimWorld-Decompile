using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class Trigger_WoundedGuestPresent : Trigger
	{
		private const int CheckInterval = 800;

		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

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
	}
}

using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class Trigger_WoundedGuestPresent : Trigger
	{
		private const int CheckInterval = 800;

		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)base.data;
			}
		}

		public Trigger_WoundedGuestPresent()
		{
			base.data = new TriggerData_PawnCycleInd();
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 800 == 0)
			{
				TriggerData_PawnCycleInd data = this.Data;
				data.pawnCycleInd++;
				if (data.pawnCycleInd >= lord.ownedPawns.Count)
				{
					data.pawnCycleInd = 0;
				}
				if (lord.ownedPawns.Any())
				{
					Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
					if (pawn.Spawned && !pawn.Downed && !pawn.InMentalState && KidnapAIUtility.ReachableWoundedGuest(pawn) != null)
					{
						result = true;
						goto IL_00b7;
					}
				}
			}
			result = false;
			goto IL_00b7;
			IL_00b7:
			return result;
		}
	}
}

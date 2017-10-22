using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class Trigger_KidnapVictimPresent : Trigger
	{
		private const int CheckInterval = 120;

		private const int MinTicksSinceDamage = 300;

		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)base.data;
			}
		}

		public Trigger_KidnapVictimPresent()
		{
			base.data = new TriggerData_PawnCycleInd();
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0 && Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
			{
				TriggerData_PawnCycleInd data = this.Data;
				data.pawnCycleInd++;
				if (data.pawnCycleInd >= lord.ownedPawns.Count)
				{
					data.pawnCycleInd = 0;
				}
				Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
				Pawn pawn2 = default(Pawn);
				if (pawn.Spawned && !pawn.Downed && pawn.MentalStateDef == null && KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, (List<Thing>)null) && !GenAI.InDangerousCombat(pawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}

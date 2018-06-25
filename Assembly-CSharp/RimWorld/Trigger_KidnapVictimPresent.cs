using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class Trigger_KidnapVictimPresent : Trigger
	{
		private const int CheckInterval = 120;

		private const int MinTicksSinceDamage = 300;

		public Trigger_KidnapVictimPresent()
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
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (this.data == null || !(this.data is TriggerData_PawnCycleInd))
				{
					BackCompatibility.TriggerDataPawnCycleIndNull(this);
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
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
						if (pawn.Spawned && !pawn.Downed && pawn.MentalStateDef == null)
						{
							Pawn pawn2;
							if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, null) && !GenAI.InDangerousCombat(pawn))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
	}
}

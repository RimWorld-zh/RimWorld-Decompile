using System;

namespace Verse.AI.Group
{
	public class Trigger_PawnKilled : Trigger
	{
		public Trigger_PawnKilled()
		{
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && signal.condition == PawnLostCondition.IncappedOrKilled && signal.Pawn.Dead;
		}
	}
}

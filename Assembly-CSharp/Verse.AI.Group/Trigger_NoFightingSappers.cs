using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_NoFightingSappers : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn p = lord.ownedPawns[i];
					if (this.IsFightingSapper(p))
						goto IL_0030;
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_005b;
			IL_005b:
			return result;
			IL_0030:
			result = false;
			goto IL_005b;
		}

		private bool IsFightingSapper(Pawn p)
		{
			return (byte)((!p.Downed && !p.InMentalState) ? (RaidStrategyWorker_ImmediateAttackSappers.CanBeSapper(p.kindDef) ? 1 : 0) : 0) != 0;
		}
	}
}

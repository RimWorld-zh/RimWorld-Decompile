using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_TraderAndAllTraderCaravanGuardsLost : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				int num = 0;
				while (num < lord.ownedPawns.Count)
				{
					TraderCaravanRole traderCaravanRole = lord.ownedPawns[num].GetTraderCaravanRole();
					if (traderCaravanRole != TraderCaravanRole.Trader && traderCaravanRole != TraderCaravanRole.Guard)
					{
						num++;
						continue;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}
}

using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_TraderAndAllTraderCaravanGuardsLost : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
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
					goto IL_0037;
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_0062;
			IL_0062:
			return result;
			IL_0037:
			result = false;
			goto IL_0062;
		}
	}
}

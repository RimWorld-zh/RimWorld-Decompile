using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2A RID: 2602
	public class Trigger_TraderAndAllTraderCaravanGuardsLost : Trigger
	{
		// Token: 0x060039D4 RID: 14804 RVA: 0x001E8DF4 File Offset: 0x001E71F4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					TraderCaravanRole traderCaravanRole = lord.ownedPawns[i].GetTraderCaravanRole();
					if (traderCaravanRole == TraderCaravanRole.Trader || traderCaravanRole == TraderCaravanRole.Guard)
					{
						return false;
					}
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}

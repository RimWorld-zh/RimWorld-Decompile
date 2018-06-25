using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2C RID: 2604
	public class Trigger_TraderAndAllTraderCaravanGuardsLost : Trigger
	{
		// Token: 0x060039D8 RID: 14808 RVA: 0x001E8F20 File Offset: 0x001E7320
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

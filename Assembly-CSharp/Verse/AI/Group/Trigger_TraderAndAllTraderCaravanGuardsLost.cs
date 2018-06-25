using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2D RID: 2605
	public class Trigger_TraderAndAllTraderCaravanGuardsLost : Trigger
	{
		// Token: 0x060039D9 RID: 14809 RVA: 0x001E924C File Offset: 0x001E764C
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

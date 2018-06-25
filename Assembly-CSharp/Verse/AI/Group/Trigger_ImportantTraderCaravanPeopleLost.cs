using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2B RID: 2603
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x060039D6 RID: 14806 RVA: 0x001E8E78 File Offset: 0x001E7278
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.IncappedOrKilled || signal.condition == PawnLostCondition.MadePrisoner))
			{
				TraderCaravanRole traderCaravanRole = signal.Pawn.GetTraderCaravanRole();
				if (traderCaravanRole == TraderCaravanRole.Trader || signal.Pawn.RaceProps.packAnimal)
				{
					return true;
				}
				if (lord.numPawnsLostViolently > 0 && (float)lord.numPawnsLostViolently / (float)lord.numPawnsEverGained >= 0.5f)
				{
					return true;
				}
			}
			return false;
		}
	}
}

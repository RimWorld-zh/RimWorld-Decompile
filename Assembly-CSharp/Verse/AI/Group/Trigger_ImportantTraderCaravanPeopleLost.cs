using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2C RID: 2604
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x060039D7 RID: 14807 RVA: 0x001E91A4 File Offset: 0x001E75A4
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

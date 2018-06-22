using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A29 RID: 2601
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x060039D2 RID: 14802 RVA: 0x001E8D4C File Offset: 0x001E714C
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

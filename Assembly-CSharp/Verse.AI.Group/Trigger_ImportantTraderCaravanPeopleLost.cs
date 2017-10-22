using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.IncappedOrKilled || signal.condition == PawnLostCondition.MadePrisoner))
			{
				TraderCaravanRole traderCaravanRole = signal.Pawn.GetTraderCaravanRole();
				if (traderCaravanRole != TraderCaravanRole.Trader && traderCaravanRole != TraderCaravanRole.Carrier)
				{
					if (lord.numPawnsLostViolently > 0 && (float)lord.numPawnsLostViolently / (float)lord.numPawnsEverGained >= 0.5)
					{
						result = true;
						goto IL_0080;
					}
					goto IL_0079;
				}
				result = true;
				goto IL_0080;
			}
			goto IL_0079;
			IL_0080:
			return result;
			IL_0079:
			result = false;
			goto IL_0080;
		}
	}
}

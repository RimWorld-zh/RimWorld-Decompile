using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class Trigger_HighValueThingsAround : Trigger
	{
		private const int CheckInterval = 120;

		private const int MinTicksSinceDamage = 300;

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (TutorSystem.TutorialMode)
				{
					result = false;
					goto IL_0073;
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					float num = StealAIUtility.TotalMarketValueAround(lord.ownedPawns);
					float num2 = StealAIUtility.StartStealingMarketValueThreshold(lord);
					result = (num > num2);
					goto IL_0073;
				}
			}
			result = false;
			goto IL_0073;
			IL_0073:
			return result;
		}
	}
}

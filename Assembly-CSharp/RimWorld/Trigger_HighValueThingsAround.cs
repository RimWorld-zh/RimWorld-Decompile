using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001AD RID: 429
	public class Trigger_HighValueThingsAround : Trigger
	{
		// Token: 0x060008DC RID: 2268 RVA: 0x000537F0 File Offset: 0x00051BF0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (TutorSystem.TutorialMode)
				{
					return false;
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					float num = StealAIUtility.TotalMarketValueAround(lord.ownedPawns);
					float num2 = StealAIUtility.StartStealingMarketValueThreshold(lord);
					return num > num2;
				}
			}
			return false;
		}

		// Token: 0x040003BD RID: 957
		private const int CheckInterval = 120;

		// Token: 0x040003BE RID: 958
		private const int MinTicksSinceDamage = 300;
	}
}

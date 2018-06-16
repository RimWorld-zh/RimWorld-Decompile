using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A27 RID: 2599
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x060039CA RID: 14794 RVA: 0x001E877C File Offset: 0x001E6B7C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].needs.food.CurCategory >= HungerCategory.UrgentlyHungry)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

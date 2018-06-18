using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A27 RID: 2599
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x060039CC RID: 14796 RVA: 0x001E8850 File Offset: 0x001E6C50
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

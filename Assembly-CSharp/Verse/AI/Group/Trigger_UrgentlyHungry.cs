using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A23 RID: 2595
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x060039C6 RID: 14790 RVA: 0x001E8A90 File Offset: 0x001E6E90
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

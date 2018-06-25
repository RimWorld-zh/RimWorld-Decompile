using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A26 RID: 2598
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x060039CB RID: 14795 RVA: 0x001E8EE8 File Offset: 0x001E72E8
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

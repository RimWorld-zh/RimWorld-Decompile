using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A23 RID: 2595
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x060039C6 RID: 14790 RVA: 0x001E8AE4 File Offset: 0x001E6EE4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

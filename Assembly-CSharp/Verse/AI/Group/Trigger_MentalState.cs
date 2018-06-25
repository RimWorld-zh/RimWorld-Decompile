using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A24 RID: 2596
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x060039C7 RID: 14791 RVA: 0x001E8E10 File Offset: 0x001E7210
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

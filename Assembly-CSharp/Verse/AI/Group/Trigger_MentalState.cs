using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A21 RID: 2593
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x001E89B8 File Offset: 0x001E6DB8
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

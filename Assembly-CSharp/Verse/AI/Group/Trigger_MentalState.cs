using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A25 RID: 2597
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x060039C6 RID: 14790 RVA: 0x001E86A4 File Offset: 0x001E6AA4
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

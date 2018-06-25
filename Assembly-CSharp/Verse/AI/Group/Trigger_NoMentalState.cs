using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A25 RID: 2597
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x060039C9 RID: 14793 RVA: 0x001E8E78 File Offset: 0x001E7278
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return false;
					}
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}

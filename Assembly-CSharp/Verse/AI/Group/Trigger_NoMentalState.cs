using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A26 RID: 2598
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x060039CA RID: 14794 RVA: 0x001E87E0 File Offset: 0x001E6BE0
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

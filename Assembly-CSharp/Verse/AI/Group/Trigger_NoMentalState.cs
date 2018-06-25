using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A24 RID: 2596
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x060039C8 RID: 14792 RVA: 0x001E8B4C File Offset: 0x001E6F4C
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

using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A22 RID: 2594
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x060039C4 RID: 14788 RVA: 0x001E8A20 File Offset: 0x001E6E20
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

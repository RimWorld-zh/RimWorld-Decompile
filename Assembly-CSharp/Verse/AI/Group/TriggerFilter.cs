using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A0D RID: 2573
	public abstract class TriggerFilter
	{
		// Token: 0x06003991 RID: 14737
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}

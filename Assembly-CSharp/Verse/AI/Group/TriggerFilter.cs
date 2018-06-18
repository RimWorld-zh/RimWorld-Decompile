using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A0D RID: 2573
	public abstract class TriggerFilter
	{
		// Token: 0x06003993 RID: 14739
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}

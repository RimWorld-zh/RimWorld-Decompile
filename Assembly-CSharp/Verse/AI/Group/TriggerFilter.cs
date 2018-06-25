using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A0C RID: 2572
	public abstract class TriggerFilter
	{
		// Token: 0x06003992 RID: 14738
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}

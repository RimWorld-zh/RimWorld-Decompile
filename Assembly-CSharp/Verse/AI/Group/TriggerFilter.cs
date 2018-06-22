using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A09 RID: 2569
	public abstract class TriggerFilter
	{
		// Token: 0x0600398D RID: 14733
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}

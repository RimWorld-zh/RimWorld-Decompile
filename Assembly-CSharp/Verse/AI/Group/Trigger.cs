using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A0B RID: 2571
	public abstract class Trigger
	{
		// Token: 0x0600398B RID: 14731
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x0600398C RID: 14732 RVA: 0x0005369B File Offset: 0x00051A9B
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x000536A0 File Offset: 0x00051AA0
		public override string ToString()
		{
			return base.GetType().ToString();
		}

		// Token: 0x04002495 RID: 9365
		public TriggerData data;

		// Token: 0x04002496 RID: 9366
		public List<TriggerFilter> filters;
	}
}

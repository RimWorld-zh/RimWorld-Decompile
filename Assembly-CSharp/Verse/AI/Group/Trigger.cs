using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A09 RID: 2569
	public abstract class Trigger
	{
		// Token: 0x04002491 RID: 9361
		public TriggerData data;

		// Token: 0x04002492 RID: 9362
		public List<TriggerFilter> filters;

		// Token: 0x0600398B RID: 14731
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x0600398C RID: 14732 RVA: 0x000536AF File Offset: 0x00051AAF
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x000536B4 File Offset: 0x00051AB4
		public override string ToString()
		{
			return base.GetType().ToString();
		}
	}
}

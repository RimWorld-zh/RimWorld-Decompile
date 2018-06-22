using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A07 RID: 2567
	public abstract class Trigger
	{
		// Token: 0x06003987 RID: 14727
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x06003988 RID: 14728 RVA: 0x000536AF File Offset: 0x00051AAF
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x000536B4 File Offset: 0x00051AB4
		public override string ToString()
		{
			return base.GetType().ToString();
		}

		// Token: 0x04002490 RID: 9360
		public TriggerData data;

		// Token: 0x04002491 RID: 9361
		public List<TriggerFilter> filters;
	}
}

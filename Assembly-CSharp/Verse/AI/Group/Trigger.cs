using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A0A RID: 2570
	public abstract class Trigger
	{
		// Token: 0x040024A1 RID: 9377
		public TriggerData data;

		// Token: 0x040024A2 RID: 9378
		public List<TriggerFilter> filters;

		// Token: 0x0600398C RID: 14732
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x0600398D RID: 14733 RVA: 0x000536AB File Offset: 0x00051AAB
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x000536B0 File Offset: 0x00051AB0
		public override string ToString()
		{
			return base.GetType().ToString();
		}
	}
}

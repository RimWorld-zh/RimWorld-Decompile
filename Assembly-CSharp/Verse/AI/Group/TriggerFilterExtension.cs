using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A0F RID: 2575
	public static class TriggerFilterExtension
	{
		// Token: 0x06003996 RID: 14742 RVA: 0x001E7C80 File Offset: 0x001E6080
		public static Trigger WithFilter(this Trigger t, TriggerFilter f)
		{
			if (t.filters == null)
			{
				t.filters = new List<TriggerFilter>();
			}
			t.filters.Add(f);
			return t;
		}
	}
}

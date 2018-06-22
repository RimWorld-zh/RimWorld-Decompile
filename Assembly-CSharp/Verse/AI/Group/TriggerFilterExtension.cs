using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A0B RID: 2571
	public static class TriggerFilterExtension
	{
		// Token: 0x06003990 RID: 14736 RVA: 0x001E7EC0 File Offset: 0x001E62C0
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

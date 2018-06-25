using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000A0E RID: 2574
	public static class TriggerFilterExtension
	{
		// Token: 0x06003995 RID: 14741 RVA: 0x001E8318 File Offset: 0x001E6718
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

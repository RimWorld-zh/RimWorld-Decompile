using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DF9 RID: 3577
	public interface IThingHolder
	{
		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x0600509D RID: 20637
		IThingHolder ParentHolder { get; }

		// Token: 0x0600509E RID: 20638
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x0600509F RID: 20639
		ThingOwner GetDirectlyHeldThings();
	}
}

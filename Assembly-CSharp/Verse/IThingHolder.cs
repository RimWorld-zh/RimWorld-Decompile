using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DF6 RID: 3574
	public interface IThingHolder
	{
		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06005099 RID: 20633
		IThingHolder ParentHolder { get; }

		// Token: 0x0600509A RID: 20634
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x0600509B RID: 20635
		ThingOwner GetDirectlyHeldThings();
	}
}

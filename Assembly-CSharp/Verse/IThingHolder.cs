using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DFA RID: 3578
	public interface IThingHolder
	{
		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06005087 RID: 20615
		IThingHolder ParentHolder { get; }

		// Token: 0x06005088 RID: 20616
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x06005089 RID: 20617
		ThingOwner GetDirectlyHeldThings();
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DF9 RID: 3577
	public interface IThingHolder
	{
		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06005085 RID: 20613
		IThingHolder ParentHolder { get; }

		// Token: 0x06005086 RID: 20614
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x06005087 RID: 20615
		ThingOwner GetDirectlyHeldThings();
	}
}

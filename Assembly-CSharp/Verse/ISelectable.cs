using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EA6 RID: 3750
	public interface ISelectable
	{
		// Token: 0x06005857 RID: 22615
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x06005858 RID: 22616
		string GetInspectString();

		// Token: 0x06005859 RID: 22617
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EA5 RID: 3749
	public interface ISelectable
	{
		// Token: 0x06005855 RID: 22613
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x06005856 RID: 22614
		string GetInspectString();

		// Token: 0x06005857 RID: 22615
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}

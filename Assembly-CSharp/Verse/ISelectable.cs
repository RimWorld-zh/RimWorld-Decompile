using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EA4 RID: 3748
	public interface ISelectable
	{
		// Token: 0x06005875 RID: 22645
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x06005876 RID: 22646
		string GetInspectString();

		// Token: 0x06005877 RID: 22647
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}

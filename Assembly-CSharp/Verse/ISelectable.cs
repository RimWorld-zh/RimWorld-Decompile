using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EA7 RID: 3751
	public interface ISelectable
	{
		// Token: 0x06005879 RID: 22649
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x0600587A RID: 22650
		string GetInspectString();

		// Token: 0x0600587B RID: 22651
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}

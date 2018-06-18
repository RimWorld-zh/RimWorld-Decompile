using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FCD RID: 4045
	public interface IVerbOwner
	{
		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060061C3 RID: 25027
		VerbTracker VerbTracker { get; }

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060061C4 RID: 25028
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060061C5 RID: 25029
		List<Tool> Tools { get; }

		// Token: 0x060061C6 RID: 25030
		string UniqueVerbOwnerID();
	}
}

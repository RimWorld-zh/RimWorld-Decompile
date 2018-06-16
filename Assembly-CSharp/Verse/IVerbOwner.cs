using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FCE RID: 4046
	public interface IVerbOwner
	{
		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060061C5 RID: 25029
		VerbTracker VerbTracker { get; }

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060061C6 RID: 25030
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060061C7 RID: 25031
		List<Tool> Tools { get; }

		// Token: 0x060061C8 RID: 25032
		string UniqueVerbOwnerID();
	}
}

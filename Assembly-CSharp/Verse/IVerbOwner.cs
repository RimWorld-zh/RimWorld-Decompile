using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FD2 RID: 4050
	public interface IVerbOwner
	{
		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060061FC RID: 25084
		VerbTracker VerbTracker { get; }

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060061FD RID: 25085
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060061FE RID: 25086
		List<Tool> Tools { get; }

		// Token: 0x060061FF RID: 25087
		string UniqueVerbOwnerID();
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FCE RID: 4046
	public interface IVerbOwner
	{
		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x060061EC RID: 25068
		VerbTracker VerbTracker { get; }

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060061ED RID: 25069
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060061EE RID: 25070
		List<Tool> Tools { get; }

		// Token: 0x060061EF RID: 25071
		string UniqueVerbOwnerID();
	}
}

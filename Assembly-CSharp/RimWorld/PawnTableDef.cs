using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BA RID: 698
	public class PawnTableDef : Def
	{
		// Token: 0x040006C3 RID: 1731
		public List<PawnColumnDef> columns;

		// Token: 0x040006C4 RID: 1732
		public Type workerClass = typeof(PawnTable);

		// Token: 0x040006C5 RID: 1733
		public int minWidth = 998;
	}
}

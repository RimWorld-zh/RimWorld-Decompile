using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BA RID: 698
	public class PawnTableDef : Def
	{
		// Token: 0x040006C5 RID: 1733
		public List<PawnColumnDef> columns;

		// Token: 0x040006C6 RID: 1734
		public Type workerClass = typeof(PawnTable);

		// Token: 0x040006C7 RID: 1735
		public int minWidth = 998;
	}
}

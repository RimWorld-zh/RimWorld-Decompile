using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089D RID: 2205
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06003284 RID: 12932 RVA: 0x001B2B5F File Offset: 0x001B0F5F
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x001B2B70 File Offset: 0x001B0F70
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}
	}
}

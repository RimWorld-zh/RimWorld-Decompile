using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089B RID: 2203
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06003280 RID: 12928 RVA: 0x001B2A1F File Offset: 0x001B0E1F
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x001B2A30 File Offset: 0x001B0E30
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}
	}
}

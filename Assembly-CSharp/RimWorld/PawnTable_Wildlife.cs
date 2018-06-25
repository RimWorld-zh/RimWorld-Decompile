using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089D RID: 2205
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06003283 RID: 12931 RVA: 0x001B2DC7 File Offset: 0x001B11C7
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x001B2DD8 File Offset: 0x001B11D8
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}
	}
}

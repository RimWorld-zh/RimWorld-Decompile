using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089F RID: 2207
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06003287 RID: 12935 RVA: 0x001B2837 File Offset: 0x001B0C37
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x001B2848 File Offset: 0x001B0C48
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}
	}
}

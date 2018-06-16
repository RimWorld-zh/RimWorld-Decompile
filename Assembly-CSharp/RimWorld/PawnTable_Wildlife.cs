using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089F RID: 2207
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06003285 RID: 12933 RVA: 0x001B276F File Offset: 0x001B0B6F
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x001B2780 File Offset: 0x001B0B80
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089C RID: 2204
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x06003282 RID: 12930 RVA: 0x001B2B33 File Offset: 0x001B0F33
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x001B2B44 File Offset: 0x001B0F44
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}
	}
}

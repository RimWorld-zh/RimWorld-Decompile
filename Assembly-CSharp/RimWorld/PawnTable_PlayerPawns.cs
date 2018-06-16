using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089E RID: 2206
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x06003283 RID: 12931 RVA: 0x001B2743 File Offset: 0x001B0B43
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x001B2754 File Offset: 0x001B0B54
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}
	}
}

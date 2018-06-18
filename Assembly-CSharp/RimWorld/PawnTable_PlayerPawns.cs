using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089E RID: 2206
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x06003285 RID: 12933 RVA: 0x001B280B File Offset: 0x001B0C0B
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x001B281C File Offset: 0x001B0C1C
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}
	}
}

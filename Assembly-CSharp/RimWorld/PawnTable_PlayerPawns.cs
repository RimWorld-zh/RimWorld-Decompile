using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089A RID: 2202
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x0600327E RID: 12926 RVA: 0x001B29F3 File Offset: 0x001B0DF3
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x001B2A04 File Offset: 0x001B0E04
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}
	}
}

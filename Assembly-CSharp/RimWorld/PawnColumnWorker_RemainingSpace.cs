using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088D RID: 2189
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x060031F3 RID: 12787 RVA: 0x001AF4C6 File Offset: 0x001AD8C6
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x001AF4CC File Offset: 0x001AD8CC
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x001AF4E4 File Offset: 0x001AD8E4
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x001AF500 File Offset: 0x001AD900
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x001AF51C File Offset: 0x001AD91C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

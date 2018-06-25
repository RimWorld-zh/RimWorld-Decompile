using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088D RID: 2189
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x060031F4 RID: 12788 RVA: 0x001AF25E File Offset: 0x001AD65E
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x001AF264 File Offset: 0x001AD664
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x001AF27C File Offset: 0x001AD67C
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x001AF298 File Offset: 0x001AD698
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x001AF2B4 File Offset: 0x001AD6B4
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

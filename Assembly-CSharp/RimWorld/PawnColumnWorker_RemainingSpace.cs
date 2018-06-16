using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088F RID: 2191
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x060031F5 RID: 12789 RVA: 0x001AEE6E File Offset: 0x001AD26E
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x001AEE74 File Offset: 0x001AD274
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x001AEE8C File Offset: 0x001AD28C
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x001AEEA8 File Offset: 0x001AD2A8
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x001AEEC4 File Offset: 0x001AD2C4
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

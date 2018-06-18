using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088F RID: 2191
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x060031F7 RID: 12791 RVA: 0x001AEF36 File Offset: 0x001AD336
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x001AEF3C File Offset: 0x001AD33C
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x001AEF54 File Offset: 0x001AD354
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x001AEF70 File Offset: 0x001AD370
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x001AEF8C File Offset: 0x001AD38C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088B RID: 2187
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x060031F0 RID: 12784 RVA: 0x001AF11E File Offset: 0x001AD51E
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x001AF124 File Offset: 0x001AD524
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x001AF13C File Offset: 0x001AD53C
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x001AF158 File Offset: 0x001AD558
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x001AF174 File Offset: 0x001AD574
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

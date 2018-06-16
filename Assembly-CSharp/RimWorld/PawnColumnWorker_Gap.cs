using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088E RID: 2190
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x001AEDDC File Offset: 0x001AD1DC
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x001AEDFC File Offset: 0x001AD1FC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x001AEE00 File Offset: 0x001AD200
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x001AEE28 File Offset: 0x001AD228
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x001AEE50 File Offset: 0x001AD250
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

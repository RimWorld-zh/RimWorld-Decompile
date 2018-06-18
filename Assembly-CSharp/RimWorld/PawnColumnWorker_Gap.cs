using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088E RID: 2190
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x001AEEA4 File Offset: 0x001AD2A4
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x001AEEC4 File Offset: 0x001AD2C4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x001AEEC8 File Offset: 0x001AD2C8
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x001AEEF0 File Offset: 0x001AD2F0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x001AEF18 File Offset: 0x001AD318
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

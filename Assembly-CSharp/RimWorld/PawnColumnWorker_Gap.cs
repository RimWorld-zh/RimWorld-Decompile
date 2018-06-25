using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088C RID: 2188
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x001AF1CC File Offset: 0x001AD5CC
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x001AF1EC File Offset: 0x001AD5EC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x001AF1F0 File Offset: 0x001AD5F0
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x001AF218 File Offset: 0x001AD618
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x001AF240 File Offset: 0x001AD640
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

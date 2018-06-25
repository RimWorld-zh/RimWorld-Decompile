using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088C RID: 2188
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060031ED RID: 12781 RVA: 0x001AF434 File Offset: 0x001AD834
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x001AF454 File Offset: 0x001AD854
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x001AF458 File Offset: 0x001AD858
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x001AF480 File Offset: 0x001AD880
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x001AF4A8 File Offset: 0x001AD8A8
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

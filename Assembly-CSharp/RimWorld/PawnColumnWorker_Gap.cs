using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088A RID: 2186
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x001AF08C File Offset: 0x001AD48C
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x001AF0AC File Offset: 0x001AD4AC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x001AF0B0 File Offset: 0x001AD4B0
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x001AF0D8 File Offset: 0x001AD4D8
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x001AF100 File Offset: 0x001AD500
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000888 RID: 2184
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x060031DB RID: 12763 RVA: 0x001AF200 File Offset: 0x001AD600
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x001AF20C File Offset: 0x001AD60C
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x001AF224 File Offset: 0x001AD624
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}

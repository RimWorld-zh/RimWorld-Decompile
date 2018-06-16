using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088A RID: 2186
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x060031DD RID: 12765 RVA: 0x001AEBA8 File Offset: 0x001ACFA8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x001AEBB4 File Offset: 0x001ACFB4
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x001AEBCC File Offset: 0x001ACFCC
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}

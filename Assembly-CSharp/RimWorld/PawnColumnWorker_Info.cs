using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088A RID: 2186
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x060031DF RID: 12767 RVA: 0x001AEC70 File Offset: 0x001AD070
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x001AEC7C File Offset: 0x001AD07C
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x001AEC94 File Offset: 0x001AD094
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}

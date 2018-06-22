using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000886 RID: 2182
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x060031D8 RID: 12760 RVA: 0x001AEE58 File Offset: 0x001AD258
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x001AEE64 File Offset: 0x001AD264
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x001AEE7C File Offset: 0x001AD27C
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}

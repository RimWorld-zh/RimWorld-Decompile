using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000888 RID: 2184
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x060031DC RID: 12764 RVA: 0x001AEF98 File Offset: 0x001AD398
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x001AEFA4 File Offset: 0x001AD3A4
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x001AEFBC File Offset: 0x001AD3BC
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}

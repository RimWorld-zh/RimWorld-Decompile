using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000895 RID: 2197
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06003227 RID: 12839 RVA: 0x001B043C File Offset: 0x001AE83C
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x001B0460 File Offset: 0x001AE860
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x001B0488 File Offset: 0x001AE888
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x001B0494 File Offset: 0x001AE894
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}

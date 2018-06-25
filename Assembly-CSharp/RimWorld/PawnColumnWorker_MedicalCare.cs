using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000895 RID: 2197
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06003228 RID: 12840 RVA: 0x001B01D4 File Offset: 0x001AE5D4
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x001B01F8 File Offset: 0x001AE5F8
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x001B0220 File Offset: 0x001AE620
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x001B022C File Offset: 0x001AE62C
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000897 RID: 2199
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x0600322B RID: 12843 RVA: 0x001AFEAC File Offset: 0x001AE2AC
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x001AFED0 File Offset: 0x001AE2D0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x001AFEF8 File Offset: 0x001AE2F8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x001AFF04 File Offset: 0x001AE304
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}

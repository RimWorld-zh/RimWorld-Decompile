using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000897 RID: 2199
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06003229 RID: 12841 RVA: 0x001AFDE4 File Offset: 0x001AE1E4
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x001AFE08 File Offset: 0x001AE208
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x001AFE30 File Offset: 0x001AE230
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x001AFE3C File Offset: 0x001AE23C
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}

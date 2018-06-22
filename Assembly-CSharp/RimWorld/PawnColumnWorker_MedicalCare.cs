using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000893 RID: 2195
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06003224 RID: 12836 RVA: 0x001B0094 File Offset: 0x001AE494
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x001B00B8 File Offset: 0x001AE4B8
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x001B00E0 File Offset: 0x001AE4E0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x001B00EC File Offset: 0x001AE4EC
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}

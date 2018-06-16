using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000892 RID: 2194
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		// Token: 0x06003208 RID: 12808 RVA: 0x001AF200 File Offset: 0x001AD600
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageDrugPolicies".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x001AF28D File Offset: 0x001AD68D
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs != null)
			{
				DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect, pawn);
			}
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x001AF2A8 File Offset: 0x001AD6A8
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x001AF2D4 File Offset: 0x001AD6D4
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(354f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x001AF308 File Offset: 0x001AD708
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x001AF32C File Offset: 0x001AD72C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x001AF358 File Offset: 0x001AD758
		private int GetValueToCompare(Pawn pawn)
		{
			return (pawn.drugs != null && pawn.drugs.CurrentPolicy != null) ? pawn.drugs.CurrentPolicy.uniqueId : int.MinValue;
		}

		// Token: 0x04001AD6 RID: 6870
		private const int TopAreaHeight = 65;

		// Token: 0x04001AD7 RID: 6871
		public const int ManageDrugPoliciesButtonHeight = 32;
	}
}

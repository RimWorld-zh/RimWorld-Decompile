using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088E RID: 2190
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		// Token: 0x04001AD4 RID: 6868
		private const int TopAreaHeight = 65;

		// Token: 0x04001AD5 RID: 6869
		public const int ManageDrugPoliciesButtonHeight = 32;

		// Token: 0x06003203 RID: 12803 RVA: 0x001AF4B0 File Offset: 0x001AD8B0
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

		// Token: 0x06003204 RID: 12804 RVA: 0x001AF53D File Offset: 0x001AD93D
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs != null)
			{
				DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect, pawn);
			}
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x001AF558 File Offset: 0x001AD958
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x001AF584 File Offset: 0x001AD984
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(354f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x001AF5B8 File Offset: 0x001AD9B8
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x001AF5DC File Offset: 0x001AD9DC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x001AF608 File Offset: 0x001ADA08
		private int GetValueToCompare(Pawn pawn)
		{
			return (pawn.drugs != null && pawn.drugs.CurrentPolicy != null) ? pawn.drugs.CurrentPolicy.uniqueId : int.MinValue;
		}
	}
}

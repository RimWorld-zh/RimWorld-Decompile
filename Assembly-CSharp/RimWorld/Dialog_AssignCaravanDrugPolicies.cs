using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F5 RID: 2037
	public class Dialog_AssignCaravanDrugPolicies : Window
	{
		// Token: 0x040017C4 RID: 6084
		private Caravan caravan;

		// Token: 0x040017C5 RID: 6085
		private Vector2 scrollPos;

		// Token: 0x040017C6 RID: 6086
		private float lastHeight;

		// Token: 0x040017C7 RID: 6087
		private const float RowHeight = 30f;

		// Token: 0x040017C8 RID: 6088
		private const float AssignDrugPolicyButtonsTotalWidth = 354f;

		// Token: 0x040017C9 RID: 6089
		private const int ManageDrugPoliciesButtonHeight = 32;

		// Token: 0x06002D32 RID: 11570 RVA: 0x0017C01C File Offset: 0x0017A41C
		public Dialog_AssignCaravanDrugPolicies(Caravan caravan)
		{
			this.caravan = caravan;
			this.doCloseButton = true;
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x0017C034 File Offset: 0x0017A434
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 500f);
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x0017C058 File Offset: 0x0017A458
		public override void DoWindowContents(Rect rect)
		{
			rect.height -= this.CloseButSize.y;
			float num = 0f;
			Rect rect2 = new Rect(rect.width - 354f - 16f, num, 354f, 32f);
			if (Widgets.ButtonText(rect2, "ManageDrugPolicies".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			num += 42f;
			Rect outRect = new Rect(0f, num, rect.width, rect.height - num);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.lastHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
			float num2 = 0f;
			for (int i = 0; i < this.caravan.pawns.Count; i++)
			{
				if (this.caravan.pawns[i].drugs != null)
				{
					if (num2 + 30f >= this.scrollPos.y && num2 <= this.scrollPos.y + outRect.height)
					{
						this.DoRow(new Rect(0f, num2, viewRect.width, 30f), this.caravan.pawns[i]);
					}
					num2 += 30f;
				}
			}
			this.lastHeight = num2;
			Widgets.EndScrollView();
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x0017C1F8 File Offset: 0x0017A5F8
		private void DoRow(Rect rect, Pawn pawn)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 354f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect2, pawn.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			Rect rect3 = new Rect(rect.x + rect.width - 354f, rect.y, 354f, 30f);
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect3, pawn);
		}
	}
}

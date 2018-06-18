using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007F9 RID: 2041
	public class Dialog_AssignCaravanDrugPolicies : Window
	{
		// Token: 0x06002D39 RID: 11577 RVA: 0x0017BE44 File Offset: 0x0017A244
		public Dialog_AssignCaravanDrugPolicies(Caravan caravan)
		{
			this.caravan = caravan;
			this.doCloseButton = true;
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002D3A RID: 11578 RVA: 0x0017BE5C File Offset: 0x0017A25C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 500f);
			}
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x0017BE80 File Offset: 0x0017A280
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

		// Token: 0x06002D3C RID: 11580 RVA: 0x0017C020 File Offset: 0x0017A420
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

		// Token: 0x040017C6 RID: 6086
		private Caravan caravan;

		// Token: 0x040017C7 RID: 6087
		private Vector2 scrollPos;

		// Token: 0x040017C8 RID: 6088
		private float lastHeight;

		// Token: 0x040017C9 RID: 6089
		private const float RowHeight = 30f;

		// Token: 0x040017CA RID: 6090
		private const float AssignDrugPolicyButtonsTotalWidth = 354f;

		// Token: 0x040017CB RID: 6091
		private const int ManageDrugPoliciesButtonHeight = 32;
	}
}

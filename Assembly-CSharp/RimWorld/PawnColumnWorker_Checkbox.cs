using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200087D RID: 2173
	public abstract class PawnColumnWorker_Checkbox : PawnColumnWorker
	{
		// Token: 0x06003189 RID: 12681 RVA: 0x001ADC9C File Offset: 0x001AC09C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (this.HasCheckbox(pawn))
			{
				int num = (int)((rect.width - 24f) / 2f);
				int num2 = Mathf.Max(3, 0);
				Vector2 vector = new Vector2(rect.x + (float)num, rect.y + (float)num2);
				Rect rect2 = new Rect(vector.x, vector.y, 24f, 24f);
				bool value = this.GetValue(pawn);
				bool flag = value;
				Vector2 topLeft = vector;
				ref bool checkOn = ref value;
				bool paintable = this.def.paintable;
				Widgets.Checkbox(topLeft, ref checkOn, 24f, false, paintable, null, null);
				if (Mouse.IsOver(rect2))
				{
					string tip = this.GetTip(pawn);
					if (!tip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, tip);
					}
				}
				if (value != flag)
				{
					this.SetValue(pawn, value);
				}
			}
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x001ADD88 File Offset: 0x001AC188
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x001ADDAC File Offset: 0x001AC1AC
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x001ADDD4 File Offset: 0x001AC1D4
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x001ADDF8 File Offset: 0x001AC1F8
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x001ADE24 File Offset: 0x001AC224
		private int GetValueToCompare(Pawn pawn)
		{
			int result;
			if (!this.HasCheckbox(pawn))
			{
				result = 0;
			}
			else if (!this.GetValue(pawn))
			{
				result = 1;
			}
			else
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x001ADE60 File Offset: 0x001AC260
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x001ADE78 File Offset: 0x001AC278
		protected virtual bool HasCheckbox(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06003191 RID: 12689
		protected abstract bool GetValue(Pawn pawn);

		// Token: 0x06003192 RID: 12690
		protected abstract void SetValue(Pawn pawn, bool value);

		// Token: 0x06003193 RID: 12691 RVA: 0x001ADE90 File Offset: 0x001AC290
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (this.HasCheckbox(pawnsListForReading[i]))
					{
						if (Event.current.button == 0)
						{
							if (!this.GetValue(pawnsListForReading[i]))
							{
								this.SetValue(pawnsListForReading[i], true);
							}
						}
						else if (Event.current.button == 1)
						{
							if (this.GetValue(pawnsListForReading[i]))
							{
								this.SetValue(pawnsListForReading[i], false);
							}
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x001ADF90 File Offset: 0x001AC390
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}

		// Token: 0x04001ACD RID: 6861
		public const int HorizontalPadding = 2;
	}
}

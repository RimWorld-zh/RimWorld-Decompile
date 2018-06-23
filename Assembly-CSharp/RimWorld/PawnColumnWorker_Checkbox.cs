using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000879 RID: 2169
	public abstract class PawnColumnWorker_Checkbox : PawnColumnWorker
	{
		// Token: 0x04001ACB RID: 6859
		public const int HorizontalPadding = 2;

		// Token: 0x06003184 RID: 12676 RVA: 0x001ADF4C File Offset: 0x001AC34C
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

		// Token: 0x06003185 RID: 12677 RVA: 0x001AE038 File Offset: 0x001AC438
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x001AE05C File Offset: 0x001AC45C
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x001AE084 File Offset: 0x001AC484
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x001AE0A8 File Offset: 0x001AC4A8
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x001AE0D4 File Offset: 0x001AC4D4
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

		// Token: 0x0600318A RID: 12682 RVA: 0x001AE110 File Offset: 0x001AC510
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x001AE128 File Offset: 0x001AC528
		protected virtual bool HasCheckbox(Pawn pawn)
		{
			return true;
		}

		// Token: 0x0600318C RID: 12684
		protected abstract bool GetValue(Pawn pawn);

		// Token: 0x0600318D RID: 12685
		protected abstract void SetValue(Pawn pawn, bool value);

		// Token: 0x0600318E RID: 12686 RVA: 0x001AE140 File Offset: 0x001AC540
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

		// Token: 0x0600318F RID: 12687 RVA: 0x001AE240 File Offset: 0x001AC640
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}
	}
}

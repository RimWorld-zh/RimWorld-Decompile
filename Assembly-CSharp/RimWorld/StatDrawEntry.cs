using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081D RID: 2077
	public class StatDrawEntry
	{
		// Token: 0x06002E79 RID: 11897 RVA: 0x0018D1D4 File Offset: 0x0018B5D4
		public StatDrawEntry(StatCategoryDef category, StatDef stat, float value, StatRequest optionalReq, ToStringNumberSense numberSense = ToStringNumberSense.Undefined)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = value;
			this.valueStringInt = null;
			this.displayOrderWithinCategory = 0;
			this.optionalReq = optionalReq;
			this.hasOptionalReq = true;
			if (numberSense == ToStringNumberSense.Undefined)
			{
				this.numberSense = stat.toStringNumberSense;
			}
			else
			{
				this.numberSense = numberSense;
			}
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x0018D250 File Offset: 0x0018B650
		public StatDrawEntry(StatCategoryDef category, string label, string valueString, int displayPriorityWithinCategory = 0, string overrideReportText = "")
		{
			this.category = category;
			this.stat = null;
			this.labelInt = label;
			this.value = 0f;
			this.valueStringInt = valueString;
			this.displayOrderWithinCategory = displayPriorityWithinCategory;
			this.numberSense = ToStringNumberSense.Absolute;
			this.overrideReportText = overrideReportText;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x0018D2B0 File Offset: 0x0018B6B0
		public StatDrawEntry(StatCategoryDef category, StatDef stat)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = 0f;
			this.valueStringInt = "-";
			this.displayOrderWithinCategory = 0;
			this.numberSense = ToStringNumberSense.Undefined;
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002E7C RID: 11900 RVA: 0x0018D30C File Offset: 0x0018B70C
		public bool ShouldDisplay
		{
			get
			{
				return this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue);
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x0018D34C File Offset: 0x0018B74C
		public string LabelCap
		{
			get
			{
				string result;
				if (this.labelInt != null)
				{
					result = this.labelInt.CapitalizeFirst();
				}
				else
				{
					result = this.stat.LabelCap;
				}
				return result;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002E7E RID: 11902 RVA: 0x0018D388 File Offset: 0x0018B788
		public string ValueString
		{
			get
			{
				string result;
				if (this.numberSense == ToStringNumberSense.Factor)
				{
					result = this.value.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				}
				else if (this.valueStringInt == null)
				{
					result = this.stat.Worker.GetStatDrawEntryLabel(this.stat, this.value, this.numberSense, this.optionalReq);
				}
				else
				{
					result = this.valueStringInt;
				}
				return result;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002E7F RID: 11903 RVA: 0x0018D3FC File Offset: 0x0018B7FC
		public int DisplayPriorityWithinCategory
		{
			get
			{
				int displayPriorityInCategory;
				if (this.stat != null)
				{
					displayPriorityInCategory = this.stat.displayPriorityInCategory;
				}
				else
				{
					displayPriorityInCategory = this.displayOrderWithinCategory;
				}
				return displayPriorityInCategory;
			}
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x0018D434 File Offset: 0x0018B834
		public string GetExplanationText(StatRequest optionalReq)
		{
			string result;
			if (!this.overrideReportText.NullOrEmpty())
			{
				result = this.overrideReportText;
			}
			else if (this.stat == null)
			{
				result = "";
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.stat.LabelCap);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(this.stat.description);
				stringBuilder.AppendLine();
				if (!optionalReq.Empty)
				{
					stringBuilder.AppendLine(this.stat.Worker.GetExplanationFull(optionalReq, this.numberSense, this.value));
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			return result;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x0018D4F0 File Offset: 0x0018B8F0
		public float Draw(float x, float y, float width, bool selected, Action clickedCallback, Action mousedOverCallback, Vector2 scrollPosition, Rect scrollOutRect)
		{
			float num = width * 0.45f;
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(this.ValueString, num));
			if (y - scrollPosition.y + rect.height >= 0f && y - scrollPosition.y <= scrollOutRect.height)
			{
				if (selected)
				{
					Widgets.DrawHighlightSelected(rect);
				}
				else if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				Rect rect2 = rect;
				rect2.width -= num;
				Widgets.Label(rect2, this.LabelCap);
				Rect rect3 = rect;
				rect3.x = rect2.xMax;
				rect3.width = num;
				Widgets.Label(rect3, this.ValueString);
				if (this.stat != null)
				{
					StatDef localStat = this.stat;
					TooltipHandler.TipRegion(rect, new TipSignal(() => localStat.LabelCap + ": " + localStat.description, this.stat.GetHashCode()));
				}
				if (Widgets.ButtonInvisible(rect, false))
				{
					clickedCallback();
				}
				if (Mouse.IsOver(rect))
				{
					mousedOverCallback();
				}
			}
			return rect.height;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x0018D638 File Offset: 0x0018BA38
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.LabelCap,
				": ",
				this.ValueString,
				")"
			});
		}

		// Token: 0x040018E3 RID: 6371
		public StatCategoryDef category;

		// Token: 0x040018E4 RID: 6372
		private int displayOrderWithinCategory = 0;

		// Token: 0x040018E5 RID: 6373
		public StatDef stat;

		// Token: 0x040018E6 RID: 6374
		private float value;

		// Token: 0x040018E7 RID: 6375
		public StatRequest optionalReq;

		// Token: 0x040018E8 RID: 6376
		public bool hasOptionalReq;

		// Token: 0x040018E9 RID: 6377
		private string labelInt;

		// Token: 0x040018EA RID: 6378
		private string valueStringInt;

		// Token: 0x040018EB RID: 6379
		public string overrideReportText = null;

		// Token: 0x040018EC RID: 6380
		private ToStringNumberSense numberSense;
	}
}

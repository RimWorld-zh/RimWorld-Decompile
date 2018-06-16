using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000821 RID: 2081
	public class StatDrawEntry
	{
		// Token: 0x06002E7E RID: 11902 RVA: 0x0018CF60 File Offset: 0x0018B360
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

		// Token: 0x06002E7F RID: 11903 RVA: 0x0018CFDC File Offset: 0x0018B3DC
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

		// Token: 0x06002E80 RID: 11904 RVA: 0x0018D03C File Offset: 0x0018B43C
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

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002E81 RID: 11905 RVA: 0x0018D098 File Offset: 0x0018B498
		public bool ShouldDisplay
		{
			get
			{
				return this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue);
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002E82 RID: 11906 RVA: 0x0018D0D8 File Offset: 0x0018B4D8
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

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002E83 RID: 11907 RVA: 0x0018D114 File Offset: 0x0018B514
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

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002E84 RID: 11908 RVA: 0x0018D188 File Offset: 0x0018B588
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

		// Token: 0x06002E85 RID: 11909 RVA: 0x0018D1C0 File Offset: 0x0018B5C0
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

		// Token: 0x06002E86 RID: 11910 RVA: 0x0018D27C File Offset: 0x0018B67C
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

		// Token: 0x06002E87 RID: 11911 RVA: 0x0018D3C4 File Offset: 0x0018B7C4
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

		// Token: 0x040018E5 RID: 6373
		public StatCategoryDef category;

		// Token: 0x040018E6 RID: 6374
		private int displayOrderWithinCategory = 0;

		// Token: 0x040018E7 RID: 6375
		public StatDef stat;

		// Token: 0x040018E8 RID: 6376
		private float value;

		// Token: 0x040018E9 RID: 6377
		public StatRequest optionalReq;

		// Token: 0x040018EA RID: 6378
		public bool hasOptionalReq;

		// Token: 0x040018EB RID: 6379
		private string labelInt;

		// Token: 0x040018EC RID: 6380
		private string valueStringInt;

		// Token: 0x040018ED RID: 6381
		public string overrideReportText = null;

		// Token: 0x040018EE RID: 6382
		private ToStringNumberSense numberSense;
	}
}

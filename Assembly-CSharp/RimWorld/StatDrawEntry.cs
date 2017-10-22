using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StatDrawEntry
	{
		public StatCategoryDef category;

		private int displayOrderWithinCategory;

		public StatDef stat;

		private float value;

		public StatRequest optionalReq;

		public bool hasOptionalReq;

		private string labelInt;

		private string valueStringInt;

		public string overrideReportText;

		private ToStringNumberSense numberSense;

		public bool ShouldDisplay
		{
			get
			{
				if (this.stat != null)
				{
					return !Mathf.Approximately(this.value, this.stat.hideAtValue);
				}
				return true;
			}
		}

		public string LabelCap
		{
			get
			{
				if (this.labelInt != null)
				{
					return this.labelInt.CapitalizeFirst();
				}
				return this.stat.LabelCap;
			}
		}

		public string ValueString
		{
			get
			{
				if (this.numberSense == ToStringNumberSense.Factor)
				{
					return this.value.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				}
				if (this.stat != null)
				{
					return this.stat.Worker.GetStatDrawEntryLabel(this.stat, this.value, this.numberSense, this.optionalReq);
				}
				return this.valueStringInt;
			}
		}

		public int DisplayPriorityWithinCategory
		{
			get
			{
				if (this.stat != null)
				{
					return this.stat.displayPriorityInCategory;
				}
				return this.displayOrderWithinCategory;
			}
		}

		public StatDrawEntry(StatCategoryDef category, StatDef stat, float value, StatRequest optionalReq, ToStringNumberSense numberSense = ToStringNumberSense.Undefined)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = (string)null;
			this.value = value;
			this.valueStringInt = (string)null;
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

		public StatDrawEntry(StatCategoryDef category, string label, string valueString, int displayPriorityWithinCategory = 0)
		{
			this.category = category;
			this.stat = null;
			this.labelInt = label;
			this.value = 0f;
			this.valueStringInt = valueString;
			this.displayOrderWithinCategory = displayPriorityWithinCategory;
			this.numberSense = ToStringNumberSense.Absolute;
		}

		public string GetExplanationText(StatRequest optionalReq)
		{
			if (!this.overrideReportText.NullOrEmpty())
			{
				return this.overrideReportText;
			}
			if (this.stat == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.stat.LabelCap);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(this.stat.description);
			stringBuilder.AppendLine();
			if (!optionalReq.Empty)
			{
				stringBuilder.AppendLine(this.stat.Worker.GetExplanation(optionalReq, this.numberSense));
				this.stat.Worker.FinalizeExplanation(stringBuilder, optionalReq, this.numberSense, this.value);
			}
			return stringBuilder.ToString();
		}

		public float Draw(float x, float y, float width, bool selected, Action clickedCallback)
		{
			float num = (float)(width * 0.44999998807907104);
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(this.ValueString, num));
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
				TooltipHandler.TipRegion(rect, new TipSignal((Func<string>)(() => localStat.LabelCap + ": " + localStat.description), this.stat.GetHashCode()));
			}
			if (Widgets.ButtonInvisible(rect, false))
			{
				clickedCallback();
			}
			return rect.height;
		}

		public override string ToString()
		{
			return "(" + this.LabelCap + ": " + this.ValueString + ")";
		}
	}
}

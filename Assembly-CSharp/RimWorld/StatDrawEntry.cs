using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StatDrawEntry
	{
		public StatCategoryDef category;

		private int displayOrderWithinCategory = 0;

		public StatDef stat;

		private float value;

		public StatRequest optionalReq;

		public bool hasOptionalReq;

		private string labelInt;

		private string valueStringInt;

		public string overrideReportText = (string)null;

		private ToStringNumberSense numberSense;

		public bool ShouldDisplay
		{
			get
			{
				return this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue);
			}
		}

		public string LabelCap
		{
			get
			{
				return (this.labelInt == null) ? this.stat.LabelCap : this.labelInt.CapitalizeFirst();
			}
		}

		public string ValueString
		{
			get
			{
				return (this.numberSense != ToStringNumberSense.Factor) ? ((this.valueStringInt != null) ? this.valueStringInt : this.stat.Worker.GetStatDrawEntryLabel(this.stat, this.value, this.numberSense, this.optionalReq)) : this.value.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
			}
		}

		public int DisplayPriorityWithinCategory
		{
			get
			{
				return (this.stat == null) ? this.displayOrderWithinCategory : this.stat.displayPriorityInCategory;
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

		public StatDrawEntry(StatCategoryDef category, StatDef stat)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = (string)null;
			this.value = 0f;
			this.valueStringInt = "-";
			this.displayOrderWithinCategory = 0;
			this.numberSense = ToStringNumberSense.Undefined;
		}

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
					if (this.stat.Worker.IsDisabledFor(optionalReq.Thing))
					{
						stringBuilder.AppendLine("StatsReport_PermanentlyDisabled".Translate());
					}
					else
					{
						stringBuilder.AppendLine(this.stat.Worker.GetExplanationUnfinalized(optionalReq, this.numberSense).TrimEndNewlines());
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(this.stat.Worker.GetExplanationFinalizePart(optionalReq, this.numberSense, this.value));
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			return result;
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

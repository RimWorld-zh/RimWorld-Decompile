using System;
using System.Runtime.CompilerServices;
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

		public string overrideReportText = null;

		private ToStringNumberSense numberSense;

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
			this.labelInt = null;
			this.value = 0f;
			this.valueStringInt = "-";
			this.displayOrderWithinCategory = 0;
			this.numberSense = ToStringNumberSense.Undefined;
		}

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

		[CompilerGenerated]
		private sealed class <Draw>c__AnonStorey0
		{
			internal StatDef localStat;

			public <Draw>c__AnonStorey0()
			{
			}

			internal string <>m__0()
			{
				return this.localStat.LabelCap + ": " + this.localStat.description;
			}
		}
	}
}

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class SimpleCurveDrawerStyle
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <DrawBackground>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <DrawBackgroundLines>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <DrawMeasures>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <DrawPoints>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <DrawLegend>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <DrawCurveMousePoint>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <OnlyPositiveValues>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <UseFixedSection>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <UseFixedScale>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <UseAntiAliasedLines>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <PointsRemoveOptimization>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int <MeasureLabelsXCount>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int <MeasureLabelsYCount>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <XIntegersOnly>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <YIntegersOnly>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string <LabelX>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private FloatRange <FixedSection>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Vector2 <FixedScale>k__BackingField;

		public SimpleCurveDrawerStyle()
		{
			this.DrawBackground = false;
			this.DrawBackgroundLines = true;
			this.DrawMeasures = false;
			this.DrawPoints = true;
			this.DrawLegend = false;
			this.DrawCurveMousePoint = false;
			this.OnlyPositiveValues = false;
			this.UseFixedSection = false;
			this.UseFixedScale = false;
			this.UseAntiAliasedLines = false;
			this.PointsRemoveOptimization = false;
			this.MeasureLabelsXCount = 5;
			this.MeasureLabelsYCount = 5;
			this.XIntegersOnly = false;
			this.YIntegersOnly = false;
			this.LabelX = "x";
		}

		public bool DrawBackground
		{
			[CompilerGenerated]
			get
			{
				return this.<DrawBackground>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DrawBackground>k__BackingField = value;
			}
		}

		public bool DrawBackgroundLines
		{
			[CompilerGenerated]
			get
			{
				return this.<DrawBackgroundLines>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DrawBackgroundLines>k__BackingField = value;
			}
		}

		public bool DrawMeasures
		{
			[CompilerGenerated]
			get
			{
				return this.<DrawMeasures>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DrawMeasures>k__BackingField = value;
			}
		}

		public bool DrawPoints
		{
			[CompilerGenerated]
			get
			{
				return this.<DrawPoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DrawPoints>k__BackingField = value;
			}
		}

		public bool DrawLegend
		{
			[CompilerGenerated]
			get
			{
				return this.<DrawLegend>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DrawLegend>k__BackingField = value;
			}
		}

		public bool DrawCurveMousePoint
		{
			[CompilerGenerated]
			get
			{
				return this.<DrawCurveMousePoint>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DrawCurveMousePoint>k__BackingField = value;
			}
		}

		public bool OnlyPositiveValues
		{
			[CompilerGenerated]
			get
			{
				return this.<OnlyPositiveValues>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OnlyPositiveValues>k__BackingField = value;
			}
		}

		public bool UseFixedSection
		{
			[CompilerGenerated]
			get
			{
				return this.<UseFixedSection>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UseFixedSection>k__BackingField = value;
			}
		}

		public bool UseFixedScale
		{
			[CompilerGenerated]
			get
			{
				return this.<UseFixedScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UseFixedScale>k__BackingField = value;
			}
		}

		public bool UseAntiAliasedLines
		{
			[CompilerGenerated]
			get
			{
				return this.<UseAntiAliasedLines>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UseAntiAliasedLines>k__BackingField = value;
			}
		}

		public bool PointsRemoveOptimization
		{
			[CompilerGenerated]
			get
			{
				return this.<PointsRemoveOptimization>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PointsRemoveOptimization>k__BackingField = value;
			}
		}

		public int MeasureLabelsXCount
		{
			[CompilerGenerated]
			get
			{
				return this.<MeasureLabelsXCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MeasureLabelsXCount>k__BackingField = value;
			}
		}

		public int MeasureLabelsYCount
		{
			[CompilerGenerated]
			get
			{
				return this.<MeasureLabelsYCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MeasureLabelsYCount>k__BackingField = value;
			}
		}

		public bool XIntegersOnly
		{
			[CompilerGenerated]
			get
			{
				return this.<XIntegersOnly>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<XIntegersOnly>k__BackingField = value;
			}
		}

		public bool YIntegersOnly
		{
			[CompilerGenerated]
			get
			{
				return this.<YIntegersOnly>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<YIntegersOnly>k__BackingField = value;
			}
		}

		public string LabelX
		{
			[CompilerGenerated]
			get
			{
				return this.<LabelX>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LabelX>k__BackingField = value;
			}
		}

		public FloatRange FixedSection
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedSection>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FixedSection>k__BackingField = value;
			}
		}

		public Vector2 FixedScale
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FixedScale>k__BackingField = value;
			}
		}
	}
}

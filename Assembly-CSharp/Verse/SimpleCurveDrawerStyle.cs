using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFB RID: 3835
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x06005BA9 RID: 23465 RVA: 0x002EB1CC File Offset: 0x002E95CC
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

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06005BAA RID: 23466 RVA: 0x002EB254 File Offset: 0x002E9654
		// (set) Token: 0x06005BAB RID: 23467 RVA: 0x002EB26E File Offset: 0x002E966E
		public bool DrawBackground { get; set; }

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06005BAC RID: 23468 RVA: 0x002EB278 File Offset: 0x002E9678
		// (set) Token: 0x06005BAD RID: 23469 RVA: 0x002EB292 File Offset: 0x002E9692
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06005BAE RID: 23470 RVA: 0x002EB29C File Offset: 0x002E969C
		// (set) Token: 0x06005BAF RID: 23471 RVA: 0x002EB2B6 File Offset: 0x002E96B6
		public bool DrawMeasures { get; set; }

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x002EB2C0 File Offset: 0x002E96C0
		// (set) Token: 0x06005BB1 RID: 23473 RVA: 0x002EB2DA File Offset: 0x002E96DA
		public bool DrawPoints { get; set; }

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005BB2 RID: 23474 RVA: 0x002EB2E4 File Offset: 0x002E96E4
		// (set) Token: 0x06005BB3 RID: 23475 RVA: 0x002EB2FE File Offset: 0x002E96FE
		public bool DrawLegend { get; set; }

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005BB4 RID: 23476 RVA: 0x002EB308 File Offset: 0x002E9708
		// (set) Token: 0x06005BB5 RID: 23477 RVA: 0x002EB322 File Offset: 0x002E9722
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005BB6 RID: 23478 RVA: 0x002EB32C File Offset: 0x002E972C
		// (set) Token: 0x06005BB7 RID: 23479 RVA: 0x002EB346 File Offset: 0x002E9746
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005BB8 RID: 23480 RVA: 0x002EB350 File Offset: 0x002E9750
		// (set) Token: 0x06005BB9 RID: 23481 RVA: 0x002EB36A File Offset: 0x002E976A
		public bool UseFixedSection { get; set; }

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06005BBA RID: 23482 RVA: 0x002EB374 File Offset: 0x002E9774
		// (set) Token: 0x06005BBB RID: 23483 RVA: 0x002EB38E File Offset: 0x002E978E
		public bool UseFixedScale { get; set; }

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005BBC RID: 23484 RVA: 0x002EB398 File Offset: 0x002E9798
		// (set) Token: 0x06005BBD RID: 23485 RVA: 0x002EB3B2 File Offset: 0x002E97B2
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005BBE RID: 23486 RVA: 0x002EB3BC File Offset: 0x002E97BC
		// (set) Token: 0x06005BBF RID: 23487 RVA: 0x002EB3D6 File Offset: 0x002E97D6
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06005BC0 RID: 23488 RVA: 0x002EB3E0 File Offset: 0x002E97E0
		// (set) Token: 0x06005BC1 RID: 23489 RVA: 0x002EB3FA File Offset: 0x002E97FA
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005BC2 RID: 23490 RVA: 0x002EB404 File Offset: 0x002E9804
		// (set) Token: 0x06005BC3 RID: 23491 RVA: 0x002EB41E File Offset: 0x002E981E
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06005BC4 RID: 23492 RVA: 0x002EB428 File Offset: 0x002E9828
		// (set) Token: 0x06005BC5 RID: 23493 RVA: 0x002EB442 File Offset: 0x002E9842
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06005BC6 RID: 23494 RVA: 0x002EB44C File Offset: 0x002E984C
		// (set) Token: 0x06005BC7 RID: 23495 RVA: 0x002EB466 File Offset: 0x002E9866
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005BC8 RID: 23496 RVA: 0x002EB470 File Offset: 0x002E9870
		// (set) Token: 0x06005BC9 RID: 23497 RVA: 0x002EB48A File Offset: 0x002E988A
		public string LabelX { get; set; }

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005BCA RID: 23498 RVA: 0x002EB494 File Offset: 0x002E9894
		// (set) Token: 0x06005BCB RID: 23499 RVA: 0x002EB4AE File Offset: 0x002E98AE
		public FloatRange FixedSection { get; set; }

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005BCC RID: 23500 RVA: 0x002EB4B8 File Offset: 0x002E98B8
		// (set) Token: 0x06005BCD RID: 23501 RVA: 0x002EB4D2 File Offset: 0x002E98D2
		public Vector2 FixedScale { get; set; }
	}
}

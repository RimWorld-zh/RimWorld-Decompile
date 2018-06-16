using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFC RID: 3836
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x06005BAB RID: 23467 RVA: 0x002EB0F0 File Offset: 0x002E94F0
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

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06005BAC RID: 23468 RVA: 0x002EB178 File Offset: 0x002E9578
		// (set) Token: 0x06005BAD RID: 23469 RVA: 0x002EB192 File Offset: 0x002E9592
		public bool DrawBackground { get; set; }

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06005BAE RID: 23470 RVA: 0x002EB19C File Offset: 0x002E959C
		// (set) Token: 0x06005BAF RID: 23471 RVA: 0x002EB1B6 File Offset: 0x002E95B6
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x002EB1C0 File Offset: 0x002E95C0
		// (set) Token: 0x06005BB1 RID: 23473 RVA: 0x002EB1DA File Offset: 0x002E95DA
		public bool DrawMeasures { get; set; }

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005BB2 RID: 23474 RVA: 0x002EB1E4 File Offset: 0x002E95E4
		// (set) Token: 0x06005BB3 RID: 23475 RVA: 0x002EB1FE File Offset: 0x002E95FE
		public bool DrawPoints { get; set; }

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005BB4 RID: 23476 RVA: 0x002EB208 File Offset: 0x002E9608
		// (set) Token: 0x06005BB5 RID: 23477 RVA: 0x002EB222 File Offset: 0x002E9622
		public bool DrawLegend { get; set; }

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005BB6 RID: 23478 RVA: 0x002EB22C File Offset: 0x002E962C
		// (set) Token: 0x06005BB7 RID: 23479 RVA: 0x002EB246 File Offset: 0x002E9646
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005BB8 RID: 23480 RVA: 0x002EB250 File Offset: 0x002E9650
		// (set) Token: 0x06005BB9 RID: 23481 RVA: 0x002EB26A File Offset: 0x002E966A
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06005BBA RID: 23482 RVA: 0x002EB274 File Offset: 0x002E9674
		// (set) Token: 0x06005BBB RID: 23483 RVA: 0x002EB28E File Offset: 0x002E968E
		public bool UseFixedSection { get; set; }

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005BBC RID: 23484 RVA: 0x002EB298 File Offset: 0x002E9698
		// (set) Token: 0x06005BBD RID: 23485 RVA: 0x002EB2B2 File Offset: 0x002E96B2
		public bool UseFixedScale { get; set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005BBE RID: 23486 RVA: 0x002EB2BC File Offset: 0x002E96BC
		// (set) Token: 0x06005BBF RID: 23487 RVA: 0x002EB2D6 File Offset: 0x002E96D6
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06005BC0 RID: 23488 RVA: 0x002EB2E0 File Offset: 0x002E96E0
		// (set) Token: 0x06005BC1 RID: 23489 RVA: 0x002EB2FA File Offset: 0x002E96FA
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005BC2 RID: 23490 RVA: 0x002EB304 File Offset: 0x002E9704
		// (set) Token: 0x06005BC3 RID: 23491 RVA: 0x002EB31E File Offset: 0x002E971E
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06005BC4 RID: 23492 RVA: 0x002EB328 File Offset: 0x002E9728
		// (set) Token: 0x06005BC5 RID: 23493 RVA: 0x002EB342 File Offset: 0x002E9742
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06005BC6 RID: 23494 RVA: 0x002EB34C File Offset: 0x002E974C
		// (set) Token: 0x06005BC7 RID: 23495 RVA: 0x002EB366 File Offset: 0x002E9766
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005BC8 RID: 23496 RVA: 0x002EB370 File Offset: 0x002E9770
		// (set) Token: 0x06005BC9 RID: 23497 RVA: 0x002EB38A File Offset: 0x002E978A
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005BCA RID: 23498 RVA: 0x002EB394 File Offset: 0x002E9794
		// (set) Token: 0x06005BCB RID: 23499 RVA: 0x002EB3AE File Offset: 0x002E97AE
		public string LabelX { get; set; }

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005BCC RID: 23500 RVA: 0x002EB3B8 File Offset: 0x002E97B8
		// (set) Token: 0x06005BCD RID: 23501 RVA: 0x002EB3D2 File Offset: 0x002E97D2
		public FloatRange FixedSection { get; set; }

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06005BCE RID: 23502 RVA: 0x002EB3DC File Offset: 0x002E97DC
		// (set) Token: 0x06005BCF RID: 23503 RVA: 0x002EB3F6 File Offset: 0x002E97F6
		public Vector2 FixedScale { get; set; }
	}
}

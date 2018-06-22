using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFB RID: 3835
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x06005BD1 RID: 23505 RVA: 0x002ED200 File Offset: 0x002EB600
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

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005BD2 RID: 23506 RVA: 0x002ED288 File Offset: 0x002EB688
		// (set) Token: 0x06005BD3 RID: 23507 RVA: 0x002ED2A2 File Offset: 0x002EB6A2
		public bool DrawBackground { get; set; }

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005BD4 RID: 23508 RVA: 0x002ED2AC File Offset: 0x002EB6AC
		// (set) Token: 0x06005BD5 RID: 23509 RVA: 0x002ED2C6 File Offset: 0x002EB6C6
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x002ED2D0 File Offset: 0x002EB6D0
		// (set) Token: 0x06005BD7 RID: 23511 RVA: 0x002ED2EA File Offset: 0x002EB6EA
		public bool DrawMeasures { get; set; }

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x002ED2F4 File Offset: 0x002EB6F4
		// (set) Token: 0x06005BD9 RID: 23513 RVA: 0x002ED30E File Offset: 0x002EB70E
		public bool DrawPoints { get; set; }

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06005BDA RID: 23514 RVA: 0x002ED318 File Offset: 0x002EB718
		// (set) Token: 0x06005BDB RID: 23515 RVA: 0x002ED332 File Offset: 0x002EB732
		public bool DrawLegend { get; set; }

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005BDC RID: 23516 RVA: 0x002ED33C File Offset: 0x002EB73C
		// (set) Token: 0x06005BDD RID: 23517 RVA: 0x002ED356 File Offset: 0x002EB756
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005BDE RID: 23518 RVA: 0x002ED360 File Offset: 0x002EB760
		// (set) Token: 0x06005BDF RID: 23519 RVA: 0x002ED37A File Offset: 0x002EB77A
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06005BE0 RID: 23520 RVA: 0x002ED384 File Offset: 0x002EB784
		// (set) Token: 0x06005BE1 RID: 23521 RVA: 0x002ED39E File Offset: 0x002EB79E
		public bool UseFixedSection { get; set; }

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005BE2 RID: 23522 RVA: 0x002ED3A8 File Offset: 0x002EB7A8
		// (set) Token: 0x06005BE3 RID: 23523 RVA: 0x002ED3C2 File Offset: 0x002EB7C2
		public bool UseFixedScale { get; set; }

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x002ED3CC File Offset: 0x002EB7CC
		// (set) Token: 0x06005BE5 RID: 23525 RVA: 0x002ED3E6 File Offset: 0x002EB7E6
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x002ED3F0 File Offset: 0x002EB7F0
		// (set) Token: 0x06005BE7 RID: 23527 RVA: 0x002ED40A File Offset: 0x002EB80A
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005BE8 RID: 23528 RVA: 0x002ED414 File Offset: 0x002EB814
		// (set) Token: 0x06005BE9 RID: 23529 RVA: 0x002ED42E File Offset: 0x002EB82E
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005BEA RID: 23530 RVA: 0x002ED438 File Offset: 0x002EB838
		// (set) Token: 0x06005BEB RID: 23531 RVA: 0x002ED452 File Offset: 0x002EB852
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005BEC RID: 23532 RVA: 0x002ED45C File Offset: 0x002EB85C
		// (set) Token: 0x06005BED RID: 23533 RVA: 0x002ED476 File Offset: 0x002EB876
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06005BEE RID: 23534 RVA: 0x002ED480 File Offset: 0x002EB880
		// (set) Token: 0x06005BEF RID: 23535 RVA: 0x002ED49A File Offset: 0x002EB89A
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06005BF0 RID: 23536 RVA: 0x002ED4A4 File Offset: 0x002EB8A4
		// (set) Token: 0x06005BF1 RID: 23537 RVA: 0x002ED4BE File Offset: 0x002EB8BE
		public string LabelX { get; set; }

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06005BF2 RID: 23538 RVA: 0x002ED4C8 File Offset: 0x002EB8C8
		// (set) Token: 0x06005BF3 RID: 23539 RVA: 0x002ED4E2 File Offset: 0x002EB8E2
		public FloatRange FixedSection { get; set; }

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06005BF4 RID: 23540 RVA: 0x002ED4EC File Offset: 0x002EB8EC
		// (set) Token: 0x06005BF5 RID: 23541 RVA: 0x002ED506 File Offset: 0x002EB906
		public Vector2 FixedScale { get; set; }
	}
}

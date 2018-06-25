using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F00 RID: 3840
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x06005BDB RID: 23515 RVA: 0x002EDAA0 File Offset: 0x002EBEA0
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

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06005BDC RID: 23516 RVA: 0x002EDB28 File Offset: 0x002EBF28
		// (set) Token: 0x06005BDD RID: 23517 RVA: 0x002EDB42 File Offset: 0x002EBF42
		public bool DrawBackground { get; set; }

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005BDE RID: 23518 RVA: 0x002EDB4C File Offset: 0x002EBF4C
		// (set) Token: 0x06005BDF RID: 23519 RVA: 0x002EDB66 File Offset: 0x002EBF66
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005BE0 RID: 23520 RVA: 0x002EDB70 File Offset: 0x002EBF70
		// (set) Token: 0x06005BE1 RID: 23521 RVA: 0x002EDB8A File Offset: 0x002EBF8A
		public bool DrawMeasures { get; set; }

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005BE2 RID: 23522 RVA: 0x002EDB94 File Offset: 0x002EBF94
		// (set) Token: 0x06005BE3 RID: 23523 RVA: 0x002EDBAE File Offset: 0x002EBFAE
		public bool DrawPoints { get; set; }

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x002EDBB8 File Offset: 0x002EBFB8
		// (set) Token: 0x06005BE5 RID: 23525 RVA: 0x002EDBD2 File Offset: 0x002EBFD2
		public bool DrawLegend { get; set; }

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x002EDBDC File Offset: 0x002EBFDC
		// (set) Token: 0x06005BE7 RID: 23527 RVA: 0x002EDBF6 File Offset: 0x002EBFF6
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005BE8 RID: 23528 RVA: 0x002EDC00 File Offset: 0x002EC000
		// (set) Token: 0x06005BE9 RID: 23529 RVA: 0x002EDC1A File Offset: 0x002EC01A
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005BEA RID: 23530 RVA: 0x002EDC24 File Offset: 0x002EC024
		// (set) Token: 0x06005BEB RID: 23531 RVA: 0x002EDC3E File Offset: 0x002EC03E
		public bool UseFixedSection { get; set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06005BEC RID: 23532 RVA: 0x002EDC48 File Offset: 0x002EC048
		// (set) Token: 0x06005BED RID: 23533 RVA: 0x002EDC62 File Offset: 0x002EC062
		public bool UseFixedScale { get; set; }

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005BEE RID: 23534 RVA: 0x002EDC6C File Offset: 0x002EC06C
		// (set) Token: 0x06005BEF RID: 23535 RVA: 0x002EDC86 File Offset: 0x002EC086
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06005BF0 RID: 23536 RVA: 0x002EDC90 File Offset: 0x002EC090
		// (set) Token: 0x06005BF1 RID: 23537 RVA: 0x002EDCAA File Offset: 0x002EC0AA
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06005BF2 RID: 23538 RVA: 0x002EDCB4 File Offset: 0x002EC0B4
		// (set) Token: 0x06005BF3 RID: 23539 RVA: 0x002EDCCE File Offset: 0x002EC0CE
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005BF4 RID: 23540 RVA: 0x002EDCD8 File Offset: 0x002EC0D8
		// (set) Token: 0x06005BF5 RID: 23541 RVA: 0x002EDCF2 File Offset: 0x002EC0F2
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005BF6 RID: 23542 RVA: 0x002EDCFC File Offset: 0x002EC0FC
		// (set) Token: 0x06005BF7 RID: 23543 RVA: 0x002EDD16 File Offset: 0x002EC116
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x002EDD20 File Offset: 0x002EC120
		// (set) Token: 0x06005BF9 RID: 23545 RVA: 0x002EDD3A File Offset: 0x002EC13A
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x002EDD44 File Offset: 0x002EC144
		// (set) Token: 0x06005BFB RID: 23547 RVA: 0x002EDD5E File Offset: 0x002EC15E
		public string LabelX { get; set; }

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x002EDD68 File Offset: 0x002EC168
		// (set) Token: 0x06005BFD RID: 23549 RVA: 0x002EDD82 File Offset: 0x002EC182
		public FloatRange FixedSection { get; set; }

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06005BFE RID: 23550 RVA: 0x002EDD8C File Offset: 0x002EC18C
		// (set) Token: 0x06005BFF RID: 23551 RVA: 0x002EDDA6 File Offset: 0x002EC1A6
		public Vector2 FixedScale { get; set; }
	}
}

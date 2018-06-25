using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFF RID: 3839
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x06005BDB RID: 23515 RVA: 0x002ED880 File Offset: 0x002EBC80
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
		// (get) Token: 0x06005BDC RID: 23516 RVA: 0x002ED908 File Offset: 0x002EBD08
		// (set) Token: 0x06005BDD RID: 23517 RVA: 0x002ED922 File Offset: 0x002EBD22
		public bool DrawBackground { get; set; }

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005BDE RID: 23518 RVA: 0x002ED92C File Offset: 0x002EBD2C
		// (set) Token: 0x06005BDF RID: 23519 RVA: 0x002ED946 File Offset: 0x002EBD46
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005BE0 RID: 23520 RVA: 0x002ED950 File Offset: 0x002EBD50
		// (set) Token: 0x06005BE1 RID: 23521 RVA: 0x002ED96A File Offset: 0x002EBD6A
		public bool DrawMeasures { get; set; }

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005BE2 RID: 23522 RVA: 0x002ED974 File Offset: 0x002EBD74
		// (set) Token: 0x06005BE3 RID: 23523 RVA: 0x002ED98E File Offset: 0x002EBD8E
		public bool DrawPoints { get; set; }

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x002ED998 File Offset: 0x002EBD98
		// (set) Token: 0x06005BE5 RID: 23525 RVA: 0x002ED9B2 File Offset: 0x002EBDB2
		public bool DrawLegend { get; set; }

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x002ED9BC File Offset: 0x002EBDBC
		// (set) Token: 0x06005BE7 RID: 23527 RVA: 0x002ED9D6 File Offset: 0x002EBDD6
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005BE8 RID: 23528 RVA: 0x002ED9E0 File Offset: 0x002EBDE0
		// (set) Token: 0x06005BE9 RID: 23529 RVA: 0x002ED9FA File Offset: 0x002EBDFA
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005BEA RID: 23530 RVA: 0x002EDA04 File Offset: 0x002EBE04
		// (set) Token: 0x06005BEB RID: 23531 RVA: 0x002EDA1E File Offset: 0x002EBE1E
		public bool UseFixedSection { get; set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06005BEC RID: 23532 RVA: 0x002EDA28 File Offset: 0x002EBE28
		// (set) Token: 0x06005BED RID: 23533 RVA: 0x002EDA42 File Offset: 0x002EBE42
		public bool UseFixedScale { get; set; }

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005BEE RID: 23534 RVA: 0x002EDA4C File Offset: 0x002EBE4C
		// (set) Token: 0x06005BEF RID: 23535 RVA: 0x002EDA66 File Offset: 0x002EBE66
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06005BF0 RID: 23536 RVA: 0x002EDA70 File Offset: 0x002EBE70
		// (set) Token: 0x06005BF1 RID: 23537 RVA: 0x002EDA8A File Offset: 0x002EBE8A
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06005BF2 RID: 23538 RVA: 0x002EDA94 File Offset: 0x002EBE94
		// (set) Token: 0x06005BF3 RID: 23539 RVA: 0x002EDAAE File Offset: 0x002EBEAE
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005BF4 RID: 23540 RVA: 0x002EDAB8 File Offset: 0x002EBEB8
		// (set) Token: 0x06005BF5 RID: 23541 RVA: 0x002EDAD2 File Offset: 0x002EBED2
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005BF6 RID: 23542 RVA: 0x002EDADC File Offset: 0x002EBEDC
		// (set) Token: 0x06005BF7 RID: 23543 RVA: 0x002EDAF6 File Offset: 0x002EBEF6
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x002EDB00 File Offset: 0x002EBF00
		// (set) Token: 0x06005BF9 RID: 23545 RVA: 0x002EDB1A File Offset: 0x002EBF1A
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x002EDB24 File Offset: 0x002EBF24
		// (set) Token: 0x06005BFB RID: 23547 RVA: 0x002EDB3E File Offset: 0x002EBF3E
		public string LabelX { get; set; }

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x002EDB48 File Offset: 0x002EBF48
		// (set) Token: 0x06005BFD RID: 23549 RVA: 0x002EDB62 File Offset: 0x002EBF62
		public FloatRange FixedSection { get; set; }

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06005BFE RID: 23550 RVA: 0x002EDB6C File Offset: 0x002EBF6C
		// (set) Token: 0x06005BFF RID: 23551 RVA: 0x002EDB86 File Offset: 0x002EBF86
		public Vector2 FixedScale { get; set; }
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F3 RID: 1267
	public class Need_Chemical : Need
	{
		// Token: 0x04000D58 RID: 3416
		private const float ThreshDesire = 0.01f;

		// Token: 0x04000D59 RID: 3417
		private const float ThreshSatisfied = 0.3f;

		// Token: 0x060016CC RID: 5836 RVA: 0x000C9FEC File Offset: 0x000C83EC
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060016CD RID: 5837 RVA: 0x000CA014 File Offset: 0x000C8414
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060016CE RID: 5838 RVA: 0x000CA02C File Offset: 0x000C842C
		public DrugDesireCategory CurCategory
		{
			get
			{
				DrugDesireCategory result;
				if (this.CurLevel > 0.3f)
				{
					result = DrugDesireCategory.Satisfied;
				}
				else if (this.CurLevel > 0.01f)
				{
					result = DrugDesireCategory.Desire;
				}
				else
				{
					result = DrugDesireCategory.Withdrawal;
				}
				return result;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x000CA070 File Offset: 0x000C8470
		// (set) Token: 0x060016D0 RID: 5840 RVA: 0x000CA08C File Offset: 0x000C848C
		public override float CurLevel
		{
			get
			{
				return base.CurLevel;
			}
			set
			{
				DrugDesireCategory curCategory = this.CurCategory;
				base.CurLevel = value;
				if (this.CurCategory != curCategory)
				{
					this.CategoryChanged();
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x000CA0BC File Offset: 0x000C84BC
		public Hediff_Addiction AddictionHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
					if (hediff_Addiction != null && hediff_Addiction.def.causesNeed == this.def)
					{
						return hediff_Addiction;
					}
				}
				return null;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x000CA134 File Offset: 0x000C8534
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x000CA15A File Offset: 0x000C855A
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x000CA172 File Offset: 0x000C8572
		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000CA19C File Offset: 0x000C859C
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff != null)
			{
				addictionHediff.Notify_NeedCategoryChanged();
			}
		}
	}
}

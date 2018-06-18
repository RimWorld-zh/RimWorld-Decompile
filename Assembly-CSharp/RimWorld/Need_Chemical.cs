using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F7 RID: 1271
	public class Need_Chemical : Need
	{
		// Token: 0x060016D5 RID: 5845 RVA: 0x000C9FF4 File Offset: 0x000C83F4
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060016D6 RID: 5846 RVA: 0x000CA01C File Offset: 0x000C841C
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060016D7 RID: 5847 RVA: 0x000CA034 File Offset: 0x000C8434
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
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x000CA078 File Offset: 0x000C8478
		// (set) Token: 0x060016D9 RID: 5849 RVA: 0x000CA094 File Offset: 0x000C8494
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
		// (get) Token: 0x060016DA RID: 5850 RVA: 0x000CA0C4 File Offset: 0x000C84C4
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
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x000CA13C File Offset: 0x000C853C
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x000CA162 File Offset: 0x000C8562
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x000CA17A File Offset: 0x000C857A
		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x000CA1A4 File Offset: 0x000C85A4
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff != null)
			{
				addictionHediff.Notify_NeedCategoryChanged();
			}
		}

		// Token: 0x04000D5B RID: 3419
		private const float ThreshDesire = 0.01f;

		// Token: 0x04000D5C RID: 3420
		private const float ThreshSatisfied = 0.3f;
	}
}

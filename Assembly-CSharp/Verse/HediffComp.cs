using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D04 RID: 3332
	public class HediffComp
	{
		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06004974 RID: 18804 RVA: 0x00267DA0 File Offset: 0x002661A0
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004975 RID: 18805 RVA: 0x00267DC0 File Offset: 0x002661C0
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x00267DE0 File Offset: 0x002661E0
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x00267DF8 File Offset: 0x002661F8
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x00267E10 File Offset: 0x00266210
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x00267E2C File Offset: 0x0026622C
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00267E42 File Offset: 0x00266242
		public virtual void CompPostMake()
		{
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00267E45 File Offset: 0x00266245
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00267E48 File Offset: 0x00266248
		public virtual void CompExposeData()
		{
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00267E4B File Offset: 0x0026624B
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00267E4E File Offset: 0x0026624E
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00267E51 File Offset: 0x00266251
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x00267E54 File Offset: 0x00266254
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x00267E6A File Offset: 0x0026626A
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x00267E6D File Offset: 0x0026626D
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00267E70 File Offset: 0x00266270
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00267E73 File Offset: 0x00266273
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00267E78 File Offset: 0x00266278
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x040031DC RID: 12764
		public HediffWithComps parent;

		// Token: 0x040031DD RID: 12765
		public HediffCompProperties props;
	}
}

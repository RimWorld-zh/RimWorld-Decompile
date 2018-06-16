using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D05 RID: 3333
	public class HediffComp
	{
		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x00267DC8 File Offset: 0x002661C8
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x00267DE8 File Offset: 0x002661E8
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x00267E08 File Offset: 0x00266208
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x00267E20 File Offset: 0x00266220
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x00267E38 File Offset: 0x00266238
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600497B RID: 18811 RVA: 0x00267E54 File Offset: 0x00266254
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00267E6A File Offset: 0x0026626A
		public virtual void CompPostMake()
		{
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00267E6D File Offset: 0x0026626D
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00267E70 File Offset: 0x00266270
		public virtual void CompExposeData()
		{
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00267E73 File Offset: 0x00266273
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x00267E76 File Offset: 0x00266276
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x00267E79 File Offset: 0x00266279
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x00267E7C File Offset: 0x0026627C
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00267E92 File Offset: 0x00266292
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00267E95 File Offset: 0x00266295
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00267E98 File Offset: 0x00266298
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00267E9B File Offset: 0x0026629B
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00267EA0 File Offset: 0x002662A0
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x040031DE RID: 12766
		public HediffWithComps parent;

		// Token: 0x040031DF RID: 12767
		public HediffCompProperties props;
	}
}

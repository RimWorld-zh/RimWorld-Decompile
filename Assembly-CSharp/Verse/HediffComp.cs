using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D03 RID: 3331
	public class HediffComp
	{
		// Token: 0x040031E7 RID: 12775
		public HediffWithComps parent;

		// Token: 0x040031E8 RID: 12776
		public HediffCompProperties props;

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004988 RID: 18824 RVA: 0x00269294 File Offset: 0x00267694
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x002692B4 File Offset: 0x002676B4
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x002692D4 File Offset: 0x002676D4
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x0600498B RID: 18827 RVA: 0x002692EC File Offset: 0x002676EC
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x0600498C RID: 18828 RVA: 0x00269304 File Offset: 0x00267704
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600498D RID: 18829 RVA: 0x00269320 File Offset: 0x00267720
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00269336 File Offset: 0x00267736
		public virtual void CompPostMake()
		{
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00269339 File Offset: 0x00267739
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0026933C File Offset: 0x0026773C
		public virtual void CompExposeData()
		{
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x0026933F File Offset: 0x0026773F
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00269342 File Offset: 0x00267742
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00269345 File Offset: 0x00267745
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00269348 File Offset: 0x00267748
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x0026935E File Offset: 0x0026775E
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00269361 File Offset: 0x00267761
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00269364 File Offset: 0x00267764
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00269367 File Offset: 0x00267767
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x0026936C File Offset: 0x0026776C
		public virtual string CompDebugString()
		{
			return null;
		}
	}
}

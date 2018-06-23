using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D01 RID: 3329
	public class HediffComp
	{
		// Token: 0x040031E7 RID: 12775
		public HediffWithComps parent;

		// Token: 0x040031E8 RID: 12776
		public HediffCompProperties props;

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004985 RID: 18821 RVA: 0x002691B8 File Offset: 0x002675B8
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004986 RID: 18822 RVA: 0x002691D8 File Offset: 0x002675D8
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004987 RID: 18823 RVA: 0x002691F8 File Offset: 0x002675F8
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004988 RID: 18824 RVA: 0x00269210 File Offset: 0x00267610
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x00269228 File Offset: 0x00267628
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x00269244 File Offset: 0x00267644
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x0026925A File Offset: 0x0026765A
		public virtual void CompPostMake()
		{
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x0026925D File Offset: 0x0026765D
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00269260 File Offset: 0x00267660
		public virtual void CompExposeData()
		{
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00269263 File Offset: 0x00267663
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00269266 File Offset: 0x00267666
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x00269269 File Offset: 0x00267669
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x0026926C File Offset: 0x0026766C
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00269282 File Offset: 0x00267682
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00269285 File Offset: 0x00267685
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00269288 File Offset: 0x00267688
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x0026928B File Offset: 0x0026768B
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00269290 File Offset: 0x00267690
		public virtual string CompDebugString()
		{
			return null;
		}
	}
}

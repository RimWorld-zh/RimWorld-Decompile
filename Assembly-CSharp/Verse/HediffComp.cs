using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D04 RID: 3332
	public class HediffComp
	{
		// Token: 0x040031EE RID: 12782
		public HediffWithComps parent;

		// Token: 0x040031EF RID: 12783
		public HediffCompProperties props;

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004988 RID: 18824 RVA: 0x00269574 File Offset: 0x00267974
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x00269594 File Offset: 0x00267994
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x002695B4 File Offset: 0x002679B4
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x0600498B RID: 18827 RVA: 0x002695CC File Offset: 0x002679CC
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x0600498C RID: 18828 RVA: 0x002695E4 File Offset: 0x002679E4
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600498D RID: 18829 RVA: 0x00269600 File Offset: 0x00267A00
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00269616 File Offset: 0x00267A16
		public virtual void CompPostMake()
		{
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00269619 File Offset: 0x00267A19
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0026961C File Offset: 0x00267A1C
		public virtual void CompExposeData()
		{
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x0026961F File Offset: 0x00267A1F
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00269622 File Offset: 0x00267A22
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00269625 File Offset: 0x00267A25
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00269628 File Offset: 0x00267A28
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x0026963E File Offset: 0x00267A3E
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00269641 File Offset: 0x00267A41
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00269644 File Offset: 0x00267A44
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00269647 File Offset: 0x00267A47
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x0026964C File Offset: 0x00267A4C
		public virtual string CompDebugString()
		{
			return null;
		}
	}
}

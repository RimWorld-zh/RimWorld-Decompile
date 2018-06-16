using System;

namespace Verse
{
	// Token: 0x02000D26 RID: 3366
	public static class HediffUtility
	{
		// Token: 0x06004A08 RID: 18952 RVA: 0x0026A794 File Offset: 0x00268B94
		public static T TryGetComp<T>(this Hediff hd) where T : HediffComp
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			T result;
			if (hediffWithComps == null)
			{
				result = (T)((object)null);
			}
			else
			{
				if (hediffWithComps.comps != null)
				{
					for (int i = 0; i < hediffWithComps.comps.Count; i++)
					{
						T t = hediffWithComps.comps[i] as T;
						if (t != null)
						{
							return t;
						}
					}
				}
				result = (T)((object)null);
			}
			return result;
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x0026A81C File Offset: 0x00268C1C
		public static bool IsTended(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			bool result;
			if (hediffWithComps == null)
			{
				result = false;
			}
			else
			{
				HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
				result = (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended);
			}
			return result;
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x0026A860 File Offset: 0x00268C60
		public static bool IsPermanent(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			bool result;
			if (hediffWithComps == null)
			{
				result = false;
			}
			else
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = hediffWithComps.TryGetComp<HediffComp_GetsPermanent>();
				result = (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent);
			}
			return result;
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x0026A8A4 File Offset: 0x00268CA4
		public static bool FullyImmune(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			bool result;
			if (hediffWithComps == null)
			{
				result = false;
			}
			else
			{
				HediffComp_Immunizable hediffComp_Immunizable = hediffWithComps.TryGetComp<HediffComp_Immunizable>();
				result = (hediffComp_Immunizable != null && hediffComp_Immunizable.FullyImmune);
			}
			return result;
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x0026A8E8 File Offset: 0x00268CE8
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x0026A914 File Offset: 0x00268D14
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}
	}
}

using System;

namespace Verse
{
	// Token: 0x02000D22 RID: 3362
	public static class HediffUtility
	{
		// Token: 0x06004A17 RID: 18967 RVA: 0x0026BBA0 File Offset: 0x00269FA0
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

		// Token: 0x06004A18 RID: 18968 RVA: 0x0026BC28 File Offset: 0x0026A028
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

		// Token: 0x06004A19 RID: 18969 RVA: 0x0026BC6C File Offset: 0x0026A06C
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

		// Token: 0x06004A1A RID: 18970 RVA: 0x0026BCB0 File Offset: 0x0026A0B0
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

		// Token: 0x06004A1B RID: 18971 RVA: 0x0026BCF4 File Offset: 0x0026A0F4
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x0026BD20 File Offset: 0x0026A120
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D24 RID: 3364
	public static class HediffUtility
	{
		// Token: 0x06004A1A RID: 18970 RVA: 0x0026BC7C File Offset: 0x0026A07C
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

		// Token: 0x06004A1B RID: 18971 RVA: 0x0026BD04 File Offset: 0x0026A104
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

		// Token: 0x06004A1C RID: 18972 RVA: 0x0026BD48 File Offset: 0x0026A148
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

		// Token: 0x06004A1D RID: 18973 RVA: 0x0026BD8C File Offset: 0x0026A18C
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

		// Token: 0x06004A1E RID: 18974 RVA: 0x0026BDD0 File Offset: 0x0026A1D0
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x0026BDFC File Offset: 0x0026A1FC
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x0026BE1C File Offset: 0x0026A21C
		public static int CountAddedParts(this HediffSet hs)
		{
			int num = 0;
			List<Hediff> hediffs = hs.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i] is Hediff_AddedPart)
				{
					num++;
				}
			}
			return num;
		}
	}
}

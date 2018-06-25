using System;
using System.Collections.Generic;

namespace Verse
{
	public static class HediffUtility
	{
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

		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}

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

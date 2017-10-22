namespace Verse
{
	public static class HediffUtility
	{
		public static T TryGetComp<T>(this Hediff hd) where T : HediffComp
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			T result;
			T val;
			if (hediffWithComps == null)
			{
				result = (T)null;
			}
			else
			{
				if (hediffWithComps.comps != null)
				{
					for (int i = 0; i < hediffWithComps.comps.Count; i++)
					{
						val = (T)(hediffWithComps.comps[i] as T);
						if (val != null)
							goto IL_0050;
					}
				}
				result = (T)null;
			}
			goto IL_007a;
			IL_007a:
			return result;
			IL_0050:
			result = val;
			goto IL_007a;
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

		public static bool IsOld(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			bool result;
			if (hediffWithComps == null)
			{
				result = false;
			}
			else
			{
				HediffComp_GetsOld hediffComp_GetsOld = hediffWithComps.TryGetComp<HediffComp_GetsOld>();
				result = (hediffComp_GetsOld != null && hediffComp_GetsOld.IsOld);
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
			return hd.IsTended() && !hd.IsOld();
		}

		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsOld();
		}
	}
}

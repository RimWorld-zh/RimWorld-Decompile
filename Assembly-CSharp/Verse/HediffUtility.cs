namespace Verse
{
	public static class HediffUtility
	{
		public static T TryGetComp<T>(this Hediff hd) where T : HediffComp
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return (T)null;
			}
			if (hediffWithComps.comps != null)
			{
				for (int i = 0; i < hediffWithComps.comps.Count; i++)
				{
					T val = (T)(hediffWithComps.comps[i] as T);
					if (val != null)
					{
						return val;
					}
				}
			}
			return (T)null;
		}

		public static bool IsTended(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
			if (hediffComp_TendDuration == null)
			{
				return false;
			}
			return hediffComp_TendDuration.IsTended;
		}

		public static bool IsOld(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_GetsOld hediffComp_GetsOld = hediffWithComps.TryGetComp<HediffComp_GetsOld>();
			if (hediffComp_GetsOld == null)
			{
				return false;
			}
			return hediffComp_GetsOld.IsOld;
		}

		public static bool FullyImmune(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_Immunizable hediffComp_Immunizable = hediffWithComps.TryGetComp<HediffComp_Immunizable>();
			if (hediffComp_Immunizable == null)
			{
				return false;
			}
			return hediffComp_Immunizable.FullyImmune;
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

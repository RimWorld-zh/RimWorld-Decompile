using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public static class HediffStatsUtility
	{
		[DebuggerHidden]
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(HediffStage stage, Hediff instance)
		{
			HediffStatsUtility.<SpecialDisplayStats>c__Iterator1CA <SpecialDisplayStats>c__Iterator1CA = new HediffStatsUtility.<SpecialDisplayStats>c__Iterator1CA();
			<SpecialDisplayStats>c__Iterator1CA.instance = instance;
			<SpecialDisplayStats>c__Iterator1CA.stage = stage;
			<SpecialDisplayStats>c__Iterator1CA.<$>instance = instance;
			<SpecialDisplayStats>c__Iterator1CA.<$>stage = stage;
			HediffStatsUtility.<SpecialDisplayStats>c__Iterator1CA expr_23 = <SpecialDisplayStats>c__Iterator1CA;
			expr_23.$PC = -2;
			return expr_23;
		}
	}
}

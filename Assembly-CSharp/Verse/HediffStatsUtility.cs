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
			HediffStatsUtility.<SpecialDisplayStats>c__Iterator1D1 <SpecialDisplayStats>c__Iterator1D = new HediffStatsUtility.<SpecialDisplayStats>c__Iterator1D1();
			<SpecialDisplayStats>c__Iterator1D.instance = instance;
			<SpecialDisplayStats>c__Iterator1D.stage = stage;
			<SpecialDisplayStats>c__Iterator1D.<$>instance = instance;
			<SpecialDisplayStats>c__Iterator1D.<$>stage = stage;
			HediffStatsUtility.<SpecialDisplayStats>c__Iterator1D1 expr_23 = <SpecialDisplayStats>c__Iterator1D;
			expr_23.$PC = -2;
			return expr_23;
		}
	}
}

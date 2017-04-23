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
			HediffStatsUtility.<SpecialDisplayStats>c__Iterator1C9 <SpecialDisplayStats>c__Iterator1C = new HediffStatsUtility.<SpecialDisplayStats>c__Iterator1C9();
			<SpecialDisplayStats>c__Iterator1C.instance = instance;
			<SpecialDisplayStats>c__Iterator1C.stage = stage;
			<SpecialDisplayStats>c__Iterator1C.<$>instance = instance;
			<SpecialDisplayStats>c__Iterator1C.<$>stage = stage;
			HediffStatsUtility.<SpecialDisplayStats>c__Iterator1C9 expr_23 = <SpecialDisplayStats>c__Iterator1C;
			expr_23.$PC = -2;
			return expr_23;
		}
	}
}

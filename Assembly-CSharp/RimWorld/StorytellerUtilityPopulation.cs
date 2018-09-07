using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public static class StorytellerUtilityPopulation
	{
		private static float PopulationValue_Prisoner = 0.5f;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<float, float, string> <>f__am$cache2;

		private static StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		public static float PopulationIntent
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulation, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		public static float AdjustedPopulation
		{
			get
			{
				float num = 0f;
				num += (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
				return num + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>() * StorytellerUtilityPopulation.PopulationValue_Prisoner;
			}
		}

		private static float CalculatePopulationIntent(StorytellerDef def, float curPop, float popAdaptation)
		{
			float num = def.populationIntentFactorFromPopCurve.Evaluate(curPop);
			if (num > 0f)
			{
				num *= def.populationIntentFactorFromPopAdaptDaysCurve.Evaluate(popAdaptation);
			}
			return num;
		}

		public static string DebugReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("IntenderPopulation");
			stringBuilder.AppendLine("   Adjusted population: " + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("   Pop adaptation days: " + Find.StoryWatcher.watcherPopAdaptation.AdaptDays.ToString("F2"));
			stringBuilder.AppendLine("   PopulationIntent: " + StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			return stringBuilder.ToString();
		}

		[DebugOutput]
		public static void PopulationIntents()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add((float)i);
			}
			List<float> list2 = new List<float>();
			for (int j = 0; j < 40; j += 2)
			{
				list2.Add((float)j);
			}
			DebugTables.MakeTablesDialog<float, float>(list2, (float ds) => "d-" + ds.ToString("F0"), list, (float rv) => rv.ToString("F2"), (float ds, float p) => StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, p, (float)((int)ds)).ToString("F2"), "pop");
		}

		// Note: this type is marked as 'beforefieldinit'.
		static StorytellerUtilityPopulation()
		{
		}

		[CompilerGenerated]
		private static string <PopulationIntents>m__0(float ds)
		{
			return "d-" + ds.ToString("F0");
		}

		[CompilerGenerated]
		private static string <PopulationIntents>m__1(float rv)
		{
			return rv.ToString("F2");
		}

		[CompilerGenerated]
		private static string <PopulationIntents>m__2(float ds, float p)
		{
			return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, p, (float)((int)ds)).ToString("F2");
		}
	}
}

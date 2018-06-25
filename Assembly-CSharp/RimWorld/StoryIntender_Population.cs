using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public class StoryIntender_Population : IExposable
	{
		public Storyteller teller;

		private int lastPopGainTime = -600000;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache1;

		public StoryIntender_Population()
		{
		}

		public StoryIntender_Population(Storyteller teller)
		{
			this.teller = teller;
		}

		private StorytellerDef Def
		{
			get
			{
				return this.teller.def;
			}
		}

		private int TimeSinceLastGain
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastPopGainTime;
			}
		}

		public virtual float PopulationIntent
		{
			get
			{
				return StoryIntender_Population.CalculatePopulationIntent(this.Def, this.AdjustedPopulation, this.TimeSinceLastGain);
			}
		}

		public float AdjustedPopulation
		{
			get
			{
				float num = 0f;
				num += (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
				return num + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>() * 0.5f;
			}
		}

		public string DebugReadout
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("IntenderPopulation");
				stringBuilder.AppendLine("   AdjustedPopulation: " + this.AdjustedPopulation);
				stringBuilder.AppendLine("   PopulationIntent: " + this.PopulationIntent);
				stringBuilder.AppendLine("   lastPopGainTime: " + this.lastPopGainTime);
				stringBuilder.AppendLine("   TimeSinceLastGain: " + this.TimeSinceLastGain);
				return stringBuilder.ToString();
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastPopGainTime, "lastPopGainTime", 0, false);
		}

		public void Notify_PopulationGained()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.lastPopGainTime = Find.TickManager.TicksGame;
			}
		}

		private static float CalculatePopulationIntent(StorytellerDef def, float curPop, int ticksSinceGain)
		{
			float num = def.populationIntentFromPopCurve.Evaluate(curPop);
			if (num > 0f)
			{
				float x = (float)ticksSinceGain / 60000f;
				num *= def.populationIntentFromTimeCurve.Evaluate(x);
			}
			return num;
		}

		[DebugOutput]
		public void PopulationIntents()
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
			DebugTables.MakeTablesDialog<float, float>(list2, (float ds) => "d-" + ds.ToString("F0"), list, (float rv) => rv.ToString("F2"), (float ds, float p) => StoryIntender_Population.CalculatePopulationIntent(this.Def, p, (int)(ds * 60000f)).ToString("F2"), "pop");
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
		private string <PopulationIntents>m__2(float ds, float p)
		{
			return StoryIntender_Population.CalculatePopulationIntent(this.Def, p, (int)(ds * 60000f)).ToString("F2");
		}
	}
}

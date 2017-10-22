using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class StoryIntender_Population : IExposable
	{
		public Storyteller teller;

		private int lastPopGainTime = -600000;

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
				num += (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Colonists.Count();
				return (float)(num + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods.Count((Func<Pawn, bool>)((Pawn x) => x.IsPrisonerOfColony)) * 0.5);
			}
		}

		public string DebugReadout
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("IntenderPopulation");
				stringBuilder.AppendLine("   adjusted population: " + this.AdjustedPopulation);
				stringBuilder.AppendLine("   intent : " + this.PopulationIntent);
				stringBuilder.AppendLine("   last gain time : " + this.lastPopGainTime);
				stringBuilder.AppendLine("   time since last gain: " + this.TimeSinceLastGain);
				return stringBuilder.ToString();
			}
		}

		public StoryIntender_Population()
		{
		}

		public StoryIntender_Population(Storyteller teller)
		{
			this.teller = teller;
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
			if (num > 0.0)
			{
				float x = (float)((float)ticksSinceGain / 60000.0);
				num *= def.populationIntentFromTimeCurve.Evaluate(x);
			}
			return num;
		}

		public void DoTable_PopulationIntents()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add((float)i);
			}
			List<float> list2 = new List<float>();
			for (int num = 0; num < 40; num += 2)
			{
				list2.Add((float)num);
			}
			DebugTables.MakeTablesDialog(list2, (Func<float, string>)((float ds) => "d-" + ds.ToString("F0")), list, (Func<float, string>)((float rv) => rv.ToString("F2")), (Func<float, float, string>)((float ds, float p) => StoryIntender_Population.CalculatePopulationIntent(this.Def, p, (int)(ds * 60000.0)).ToString("F2")), "pop");
		}
	}
}

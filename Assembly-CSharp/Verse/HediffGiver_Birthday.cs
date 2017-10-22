using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class HediffGiver_Birthday : HediffGiver
	{
		public SimpleCurve ageFractionChanceCurve;

		public float averageSeverityPerDayBeforeGeneration;

		private static List<Hediff> addedHediffs = new List<Hediff>();

		public void TryApplyAndSimulateSeverityChange(Pawn pawn, float gotAtAge, bool tryNotToKillPawn)
		{
			HediffGiver_Birthday.addedHediffs.Clear();
			if (base.TryApply(pawn, HediffGiver_Birthday.addedHediffs))
			{
				if (this.averageSeverityPerDayBeforeGeneration != 0.0)
				{
					float num = (float)((pawn.ageTracker.AgeBiologicalYearsFloat - gotAtAge) * 60.0);
					if (num < 0.0)
					{
						Log.Error("daysPassed < 0, pawn=" + pawn + ", gotAtAge=" + gotAtAge);
						return;
					}
					for (int i = 0; i < HediffGiver_Birthday.addedHediffs.Count; i++)
					{
						this.SimulateSeverityChange(pawn, HediffGiver_Birthday.addedHediffs[i], num, tryNotToKillPawn);
					}
				}
				HediffGiver_Birthday.addedHediffs.Clear();
			}
		}

		private void SimulateSeverityChange(Pawn pawn, Hediff hediff, float daysPassed, bool tryNotToKillPawn)
		{
			float num = this.averageSeverityPerDayBeforeGeneration * daysPassed;
			num *= Rand.Range(0.5f, 1.4f);
			num += hediff.def.initialSeverity;
			if (tryNotToKillPawn)
			{
				this.AvoidLifeThreateningStages(ref num, hediff.def.stages);
			}
			hediff.Severity = num;
			pawn.health.Notify_HediffChanged(hediff);
		}

		private void AvoidLifeThreateningStages(ref float severity, List<HediffStage> stages)
		{
			if (!stages.NullOrEmpty())
			{
				int num = -1;
				int num2 = 0;
				while (num2 < stages.Count)
				{
					if (!stages[num2].lifeThreatening)
					{
						num2++;
						continue;
					}
					num = num2;
					break;
				}
				if (num >= 0)
				{
					if (num == 0)
					{
						severity = Mathf.Min(severity, stages[num].minSeverity);
					}
					else
					{
						severity = Mathf.Min(severity, (float)((stages[num].minSeverity + stages[num - 1].minSeverity) / 2.0));
					}
				}
			}
		}

		public float DebugChanceToHaveAtAge(Pawn pawn, int age)
		{
			float num = 1f;
			for (int num2 = 1; num2 <= age; num2++)
			{
				float x = (float)num2 / pawn.RaceProps.lifeExpectancy;
				num = (float)(num * (1.0 - this.ageFractionChanceCurve.Evaluate(x)));
			}
			return (float)(1.0 - num);
		}
	}
}

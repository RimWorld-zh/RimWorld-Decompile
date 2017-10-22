using RimWorld;
using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class HediffGiver_Hypothermia : HediffGiver
	{
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float ambientTemperature = pawn.AmbientTemperature;
			FloatRange floatRange = pawn.ComfortableTemperatureRange();
			FloatRange floatRange2 = pawn.SafeTemperatureRange();
			HediffSet hediffSet = pawn.health.hediffSet;
			Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(base.hediff, false);
			if (ambientTemperature < floatRange2.min)
			{
				float num = Mathf.Abs(ambientTemperature - floatRange2.min);
				float a = (float)(num * 6.44999963697046E-05);
				a = Mathf.Max(a, 0.00075f);
				HealthUtility.AdjustSeverity(pawn, base.hediff, a);
				if (pawn.Dead)
					return;
			}
			if (firstHediffOfDef != null)
			{
				if (ambientTemperature > floatRange.min)
				{
					float value = (float)(firstHediffOfDef.Severity * 0.027000000700354576);
					value = Mathf.Clamp(value, 0.0015f, 0.015f);
					firstHediffOfDef.Severity -= value;
				}
				else if (ambientTemperature < 0.0 && firstHediffOfDef.Severity > 0.37000000476837158)
				{
					float num2 = (float)(0.02500000037252903 * firstHediffOfDef.Severity);
					BodyPartRecord bodyPartRecord = default(BodyPartRecord);
					if (Rand.Value < num2 && (from x in pawn.RaceProps.body.AllPartsVulnerableToFrostbite
					where !hediffSet.PartIsMissing(x)
					select x).TryRandomElementByWeight<BodyPartRecord>((Func<BodyPartRecord, float>)((BodyPartRecord x) => x.def.frostbiteVulnerability), out bodyPartRecord))
					{
						int num3 = Mathf.CeilToInt((float)((float)bodyPartRecord.def.hitPoints * 0.5));
						DamageDef frostbite = DamageDefOf.Frostbite;
						int amount = num3;
						BodyPartRecord hitPart = bodyPartRecord;
						DamageInfo dinfo = new DamageInfo(frostbite, amount, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
						pawn.TakeDamage(dinfo);
					}
				}
			}
		}
	}
}

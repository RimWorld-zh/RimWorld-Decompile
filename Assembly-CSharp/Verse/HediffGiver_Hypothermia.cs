using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D37 RID: 3383
	public class HediffGiver_Hypothermia : HediffGiver
	{
		// Token: 0x0400325C RID: 12892
		public HediffDef hediffInsectoid;

		// Token: 0x06004A8D RID: 19085 RVA: 0x0026E434 File Offset: 0x0026C834
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float ambientTemperature = pawn.AmbientTemperature;
			FloatRange floatRange = pawn.ComfortableTemperatureRange();
			FloatRange floatRange2 = pawn.SafeTemperatureRange();
			HediffSet hediffSet = pawn.health.hediffSet;
			HediffDef hediffDef = (pawn.RaceProps.FleshType != FleshTypeDefOf.Insectoid) ? this.hediff : this.hediffInsectoid;
			Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(hediffDef, false);
			if (ambientTemperature < floatRange2.min)
			{
				float num = Mathf.Abs(ambientTemperature - floatRange2.min);
				float num2 = num * 6.45E-05f;
				num2 = Mathf.Max(num2, 0.00075f);
				HealthUtility.AdjustSeverity(pawn, hediffDef, num2);
				if (pawn.Dead)
				{
					return;
				}
			}
			if (firstHediffOfDef != null)
			{
				if (ambientTemperature > floatRange.min)
				{
					float num3 = firstHediffOfDef.Severity * 0.027f;
					num3 = Mathf.Clamp(num3, 0.0015f, 0.015f);
					firstHediffOfDef.Severity -= num3;
				}
				else if (pawn.RaceProps.FleshType != FleshTypeDefOf.Insectoid)
				{
					if (ambientTemperature < 0f && firstHediffOfDef.Severity > 0.37f)
					{
						float num4 = 0.025f * firstHediffOfDef.Severity;
						if (Rand.Value < num4)
						{
							BodyPartRecord bodyPartRecord;
							if ((from x in pawn.RaceProps.body.AllPartsVulnerableToFrostbite
							where !hediffSet.PartIsMissing(x)
							select x).TryRandomElementByWeight((BodyPartRecord x) => x.def.frostbiteVulnerability, out bodyPartRecord))
							{
								int num5 = Mathf.CeilToInt((float)bodyPartRecord.def.hitPoints * 0.5f);
								DamageDef frostbite = DamageDefOf.Frostbite;
								float amount = (float)num5;
								BodyPartRecord hitPart = bodyPartRecord;
								DamageInfo dinfo = new DamageInfo(frostbite, amount, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
								pawn.TakeDamage(dinfo);
							}
						}
					}
				}
			}
		}
	}
}

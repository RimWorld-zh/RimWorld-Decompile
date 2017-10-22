using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	public class HediffGiver_Heat : HediffGiver
	{
		private const int BurnCheckInterval = 420;

		public static readonly string MemoPawnBurnedByAir = "PawnBurnedByAir";

		public static readonly SimpleCurve TemperatureOverageAdjustmentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(25f, 25f),
				true
			},
			{
				new CurvePoint(50f, 40f),
				true
			},
			{
				new CurvePoint(100f, 60f),
				true
			},
			{
				new CurvePoint(200f, 80f),
				true
			},
			{
				new CurvePoint(400f, 100f),
				true
			},
			{
				new CurvePoint(4000f, 1000f),
				true
			}
		};

		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float ambientTemperature = pawn.AmbientTemperature;
			FloatRange floatRange = pawn.ComfortableTemperatureRange();
			FloatRange floatRange2 = pawn.SafeTemperatureRange();
			HediffSet hediffSet = pawn.health.hediffSet;
			Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(base.hediff, false);
			if (ambientTemperature > floatRange2.max)
			{
				float x = ambientTemperature - floatRange2.max;
				x = HediffGiver_Heat.TemperatureOverageAdjustmentCurve.Evaluate(x);
				float a = (float)(x * 6.44999963697046E-05);
				a = Mathf.Max(a, 0.000375f);
				HealthUtility.AdjustSeverity(pawn, base.hediff, a);
			}
			else if (firstHediffOfDef != null && ambientTemperature < floatRange.max)
			{
				float value = (float)(firstHediffOfDef.Severity * 0.027000000700354576);
				value = Mathf.Clamp(value, 0.0015f, 0.015f);
				firstHediffOfDef.Severity -= value;
			}
			if (!pawn.Dead && pawn.IsNestedHashIntervalTick(60, 420))
			{
				float num = (float)(floatRange.max + 150.0);
				if (ambientTemperature > num)
				{
					float x2 = ambientTemperature - num;
					x2 = HediffGiver_Heat.TemperatureOverageAdjustmentCurve.Evaluate(x2);
					int amount = Mathf.Max(GenMath.RoundRandom((float)(x2 * 0.059999998658895493)), 3);
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Burn, amount, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
					dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
					pawn.TakeDamage(dinfo);
					if (pawn.Faction == Faction.OfPlayer)
					{
						Find.TickManager.slower.SignalForceNormalSpeed();
						if (MessagesRepeatAvoider.MessageShowAllowed("PawnBeingBurned", 60f))
						{
							Messages.Message("MessagePawnBeingBurned".Translate(pawn.LabelShort).CapitalizeFirst(), (Thing)pawn, MessageTypeDefOf.ThreatSmall);
						}
					}
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.ReceiveMemo(HediffGiver_Heat.MemoPawnBurnedByAir);
					}
				}
			}
		}
	}
}

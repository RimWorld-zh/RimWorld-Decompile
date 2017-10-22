using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ShortCircuitUtility
	{
		private static Dictionary<PowerNet, bool> tmpPowerNetHasActivePowerSource = new Dictionary<PowerNet, bool>();

		private static List<IntVec3> tmpCells = new List<IntVec3>();

		public static IEnumerable<Building> GetShortCircuitablePowerConduits(Map map)
		{
			ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			try
			{
				List<Thing> conduits = map.listerThings.ThingsOfDef(ThingDefOf.PowerConduit);
				int i = 0;
				Building b;
				while (true)
				{
					if (i < conduits.Count)
					{
						b = (Building)conduits[i];
						CompPower power = b.PowerComp;
						if (power != null)
						{
							bool hasActivePowerSource;
							if (!ShortCircuitUtility.tmpPowerNetHasActivePowerSource.TryGetValue(power.PowerNet, out hasActivePowerSource))
							{
								hasActivePowerSource = power.PowerNet.HasActivePowerSource;
								ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Add(power.PowerNet, hasActivePowerSource);
							}
							if (hasActivePowerSource)
								break;
						}
						i++;
						continue;
					}
					yield break;
				}
				yield return b;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			finally
			{
				((_003CGetShortCircuitablePowerConduits_003Ec__Iterator0)/*Error near IL_0157: stateMachine*/)._003C_003E__Finally0();
			}
			IL_0167:
			/*Error near IL_0168: Unexpected return in MoveNext()*/;
		}

		public static void DoShortCircuit(Building culprit)
		{
			PowerNet powerNet = culprit.PowerComp.PowerNet;
			Map map = culprit.Map;
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			if (powerNet.batteryComps.Any((Predicate<CompPowerBattery>)((CompPowerBattery x) => x.StoredEnergy > 20.0)))
			{
				ShortCircuitUtility.DrainBatteriesAndCauseExplosion(powerNet, culprit, out num, out num2);
			}
			else
			{
				flag = ShortCircuitUtility.TryStartFireNear(culprit);
			}
			string text = (culprit.def != ThingDefOf.PowerConduit) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(culprit.Label) : "AnElectricalConduit".Translate();
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				stringBuilder.Append("ShortCircuitStartedFire".Translate(text));
			}
			else
			{
				stringBuilder.Append("ShortCircuit".Translate(text));
			}
			if (num > 0.0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitDischargedEnergy".Translate(num.ToString("F0")));
			}
			if (num2 > 5.0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasLarge".Translate());
			}
			if (num2 > 8.0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasHuge".Translate());
			}
			Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterDefOf.NegativeEvent, new TargetInfo(culprit.Position, map, false), (string)null);
		}

		public static bool TryShortCircuitInRain(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null && compPowerTrader.PowerOn && compPowerTrader.Props.shortCircuitInRain)
			{
				goto IL_0049;
			}
			if (thing.TryGetComp<CompPowerBattery>() != null && thing.TryGetComp<CompPowerBattery>().StoredEnergy > 100.0)
				goto IL_0049;
			bool result = false;
			goto IL_010e;
			IL_010e:
			return result;
			IL_0049:
			string text = "ShortCircuitRain".Translate(thing.Label);
			TargetInfo target = new TargetInfo(thing.Position, thing.Map, false);
			if (thing.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), text, LetterDefOf.NegativeEvent, target, (string)null);
			}
			else
			{
				Messages.Message(text, target, MessageTypeDefOf.NeutralEvent);
			}
			GenExplosion.DoExplosion(thing.OccupiedRect().RandomCell, thing.Map, 1.9f, DamageDefOf.Flame, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			result = true;
			goto IL_010e;
		}

		private static void DrainBatteriesAndCauseExplosion(PowerNet net, Building culprit, out float totalEnergy, out float explosionRadius)
		{
			totalEnergy = 0f;
			for (int i = 0; i < net.batteryComps.Count; i++)
			{
				CompPowerBattery compPowerBattery = net.batteryComps[i];
				totalEnergy += compPowerBattery.StoredEnergy;
				compPowerBattery.DrawPower(compPowerBattery.StoredEnergy);
			}
			explosionRadius = (float)(Mathf.Sqrt(totalEnergy) * 0.05000000074505806);
			explosionRadius = Mathf.Clamp(explosionRadius, 1.5f, 14.9f);
			GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius, DamageDefOf.Flame, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			if (explosionRadius > 3.5)
			{
				GenExplosion.DoExplosion(culprit.Position, net.Map, (float)(explosionRadius * 0.30000001192092896), DamageDefOf.Bomb, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
		}

		private static bool TryStartFireNear(Building b)
		{
			ShortCircuitUtility.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(3f);
			CellRect startRect = b.OccupiedRect();
			for (int num2 = 0; num2 < num; num2++)
			{
				IntVec3 intVec = b.Position + GenRadial.RadialPattern[num2];
				if (GenSight.LineOfSight(b.Position, intVec, b.Map, startRect, CellRect.SingleCell(intVec), null) && FireUtility.ChanceToStartFireIn(intVec, b.Map) > 0.0)
				{
					ShortCircuitUtility.tmpCells.Add(intVec);
				}
			}
			return ShortCircuitUtility.tmpCells.Any() && FireUtility.TryStartFireIn(ShortCircuitUtility.tmpCells.RandomElement(), b.Map, Rand.Range(0.1f, 1.75f));
		}
	}
}

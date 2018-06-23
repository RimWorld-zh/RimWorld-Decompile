using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000992 RID: 2450
	public static class ShortCircuitUtility
	{
		// Token: 0x0400238E RID: 9102
		private static Dictionary<PowerNet, bool> tmpPowerNetHasActivePowerSource = new Dictionary<PowerNet, bool>();

		// Token: 0x0400238F RID: 9103
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x06003714 RID: 14100 RVA: 0x001D7908 File Offset: 0x001D5D08
		public static IEnumerable<Building> GetShortCircuitablePowerConduits(Map map)
		{
			ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			try
			{
				List<Thing> conduits = map.listerThings.ThingsOfDef(ThingDefOf.PowerConduit);
				for (int i = 0; i < conduits.Count; i++)
				{
					Building b = (Building)conduits[i];
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
						{
							yield return b;
						}
					}
				}
			}
			finally
			{
				ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			}
			yield break;
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x001D7934 File Offset: 0x001D5D34
		public static void DoShortCircuit(Building culprit)
		{
			PowerNet powerNet = culprit.PowerComp.PowerNet;
			Map map = culprit.Map;
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			if (powerNet.batteryComps.Any((CompPowerBattery x) => x.StoredEnergy > 20f))
			{
				ShortCircuitUtility.DrainBatteriesAndCauseExplosion(powerNet, culprit, out num, out num2);
			}
			else
			{
				flag = ShortCircuitUtility.TryStartFireNear(culprit);
			}
			string text;
			if (culprit.def == ThingDefOf.PowerConduit)
			{
				text = "AnElectricalConduit".Translate();
			}
			else
			{
				text = Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(culprit.Label);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				stringBuilder.Append("ShortCircuitStartedFire".Translate(new object[]
				{
					text
				}));
			}
			else
			{
				stringBuilder.Append("ShortCircuit".Translate(new object[]
				{
					text
				}));
			}
			if (num > 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitDischargedEnergy".Translate(new object[]
				{
					num.ToString("F0")
				}));
			}
			if (num2 > 5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasLarge".Translate());
			}
			if (num2 > 8f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasHuge".Translate());
			}
			Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterDefOf.NegativeEvent, new TargetInfo(culprit.Position, map, false), null, null);
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x001D7B00 File Offset: 0x001D5F00
		public static bool TryShortCircuitInRain(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			bool result;
			if ((compPowerTrader != null && compPowerTrader.PowerOn && compPowerTrader.Props.shortCircuitInRain) || (thing.TryGetComp<CompPowerBattery>() != null && thing.TryGetComp<CompPowerBattery>().StoredEnergy > 100f))
			{
				string text = "ShortCircuitRain".Translate(new object[]
				{
					thing.Label
				});
				TargetInfo target = new TargetInfo(thing.Position, thing.Map, false);
				if (thing.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), text, LetterDefOf.NegativeEvent, target, null, null);
				}
				else
				{
					Messages.Message(text, target, MessageTypeDefOf.NeutralEvent, true);
				}
				GenExplosion.DoExplosion(thing.OccupiedRect().RandomCell, thing.Map, 1.9f, DamageDefOf.Flame, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x001D7C20 File Offset: 0x001D6020
		private static void DrainBatteriesAndCauseExplosion(PowerNet net, Building culprit, out float totalEnergy, out float explosionRadius)
		{
			totalEnergy = 0f;
			for (int i = 0; i < net.batteryComps.Count; i++)
			{
				CompPowerBattery compPowerBattery = net.batteryComps[i];
				totalEnergy += compPowerBattery.StoredEnergy;
				compPowerBattery.DrawPower(compPowerBattery.StoredEnergy);
			}
			explosionRadius = Mathf.Sqrt(totalEnergy) * 0.05f;
			explosionRadius = Mathf.Clamp(explosionRadius, 1.5f, 14.9f);
			GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius, DamageDefOf.Flame, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			if (explosionRadius > 3.5f)
			{
				GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius * 0.3f, DamageDefOf.Bomb, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x001D7D14 File Offset: 0x001D6114
		private static bool TryStartFireNear(Building b)
		{
			ShortCircuitUtility.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(3f);
			CellRect startRect = b.OccupiedRect();
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = b.Position + GenRadial.RadialPattern[i];
				if (GenSight.LineOfSight(b.Position, intVec, b.Map, startRect, CellRect.SingleCell(intVec), null) && FireUtility.ChanceToStartFireIn(intVec, b.Map) > 0f)
				{
					ShortCircuitUtility.tmpCells.Add(intVec);
				}
			}
			return ShortCircuitUtility.tmpCells.Any<IntVec3>() && FireUtility.TryStartFireIn(ShortCircuitUtility.tmpCells.RandomElement<IntVec3>(), b.Map, Rand.Range(0.1f, 1.75f));
		}
	}
}

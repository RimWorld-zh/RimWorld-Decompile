using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		private static List<Building_Battery> tmpBatteries = new List<Building_Battery>();

		private static List<CompPower> tmpConduits = new List<CompPower>();

		private List<Building_Battery> BatteryCandidates(Map map)
		{
			IncidentWorker_ShortCircuit.tmpBatteries.Clear();
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building_Battery building_Battery = allBuildingsColonist[i] as Building_Battery;
				if (building_Battery != null && !(building_Battery.GetComp<CompPowerBattery>().StoredEnergy < 50.0) && this.GetRandomConduit(building_Battery) != null)
				{
					IncidentWorker_ShortCircuit.tmpBatteries.Add(building_Battery);
				}
			}
			return IncidentWorker_ShortCircuit.tmpBatteries;
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			return this.BatteryCandidates(map).Any();
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Building_Battery> list = this.BatteryCandidates(map);
			if (!list.Any())
			{
				return false;
			}
			Building_Battery building_Battery = list.RandomElement();
			PowerNet powerNet = building_Battery.PowerComp.PowerNet;
			CompPower randomConduit = this.GetRandomConduit(building_Battery);
			if (randomConduit == null)
			{
				return false;
			}
			float num = 0f;
			List<CompPowerBattery>.Enumerator enumerator = powerNet.batteryComps.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CompPowerBattery current = enumerator.Current;
					num += current.StoredEnergy;
					current.DrawPower(current.StoredEnergy);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			float num2 = (float)(Mathf.Sqrt(num) * 0.05000000074505806);
			if (num2 > 14.899999618530273)
			{
				num2 = 14.9f;
			}
			ThingWithComps parent = randomConduit.parent;
			GenExplosion.DoExplosion(parent.Position, map, num2, DamageDefOf.Flame, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			if (num2 > 3.5)
			{
				GenExplosion.DoExplosion(parent.Position, map, (float)(num2 * 0.30000001192092896), DamageDefOf.Bomb, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			}
			if (!parent.Destroyed)
			{
				parent.TakeDamage(new DamageInfo(DamageDefOf.Bomb, 200, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
			string text = "something";
			if (parent.def == ThingDefOf.PowerConduit)
			{
				text = "AnElectricalConduit".Translate();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ShortCircuit".Translate(text, num.ToString("F0")));
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
			Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterDefOf.BadNonUrgent, new TargetInfo(parent.Position, map, false), (string)null);
			return true;
		}

		private CompPower GetRandomConduit(Building_Battery battery)
		{
			IncidentWorker_ShortCircuit.tmpConduits.Clear();
			List<CompPower> transmitters = battery.PowerComp.PowerNet.transmitters;
			for (int i = 0; i < transmitters.Count; i++)
			{
				if (transmitters[i].parent.def == ThingDefOf.PowerConduit)
				{
					IncidentWorker_ShortCircuit.tmpConduits.Add(transmitters[i]);
				}
			}
			if (!IncidentWorker_ShortCircuit.tmpConduits.Any())
			{
				return null;
			}
			CompPower result = IncidentWorker_ShortCircuit.tmpConduits.RandomElement();
			IncidentWorker_ShortCircuit.tmpConduits.Clear();
			return result;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ManhunterPackIncidentUtility
	{
		public static float ManhunterAnimalWeight(PawnKindDef animal, float points)
		{
			points = Mathf.Max(points, 35f);
			if (animal.combatPower > points)
			{
				return 0f;
			}
			int num = Mathf.RoundToInt(points / animal.combatPower);
			return Mathf.Clamp01(Mathf.InverseLerp(30f, 10f, (float)num));
		}

		public static bool TryFindManhunterAnimalKind(float points, int tile, out PawnKindDef animalKind)
		{
			return (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.canArriveManhunter && (tile == -1 || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k).TryRandomElementByWeight<PawnKindDef>((Func<PawnKindDef, float>)((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points)), out animalKind);
		}

		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, int tile, float points)
		{
			int num = Mathf.Max(Mathf.RoundToInt(points / animalKind.combatPower), 1);
			List<Pawn> list = new List<Pawn>();
			for (int num2 = 0; num2 < num; num2++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
				Pawn item = PawnGenerator.GeneratePawn(request);
				list.Add(item);
			}
			return list;
		}

		public static void DoTable_ManhunterResults()
		{
			List<PawnKindDef> candidates = (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.canArriveManhunter
			orderby (float)(0.0 - k.combatPower)
			select k).ToList();
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add((float)(20.0 * Mathf.Pow(1.25f, (float)i)));
			}
			DebugTables.MakeTablesDialog(list, (Func<float, string>)((float points) => points.ToString("F0") + " pts"), candidates, (Func<PawnKindDef, string>)((PawnKindDef candidate) => candidate.defName + " (" + candidate.combatPower.ToString("F0") + ")"), (Func<float, PawnKindDef, string>)delegate(float points, PawnKindDef candidate)
			{
				float num = candidates.Sum((Func<PawnKindDef, float>)((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points)));
				float num2 = ManhunterPackIncidentUtility.ManhunterAnimalWeight(candidate, points);
				if (num2 == 0.0)
				{
					return "0%";
				}
				return string.Format("{0}%, {1}", ((float)(num2 * 100.0 / num)).ToString("F0"), Mathf.Max(Mathf.RoundToInt(points / candidate.combatPower), 1));
			}, string.Empty);
		}
	}
}

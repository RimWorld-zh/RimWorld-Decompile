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
			if (animal.combatPower > 200f)
			{
				return 1f;
			}
			int num = Mathf.Max(Mathf.RoundToInt(points / animal.combatPower), 1);
			return Mathf.Clamp01(Mathf.InverseLerp(40f, 20f, (float)num));
		}

		public static bool TryFindManhunterAnimalKind(float points, int tile, out PawnKindDef animalKind)
		{
			return (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && (tile == -1 || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k).TryRandomElementByWeight((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points), out animalKind);
		}

		public static void DoTable_ManhunterResults()
		{
			List<PawnKindDef> candidates = (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal
			orderby -k.combatPower
			select k).ToList<PawnKindDef>();
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add(20f * Mathf.Pow(1.25f, (float)i));
			}
			DebugTables.MakeTablesDialog<float, PawnKindDef>(list, (float points) => points.ToString("F0") + " pts", candidates, (PawnKindDef candidate) => candidate.defName, delegate(float points, PawnKindDef candidate)
			{
				float num = candidates.Sum((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points));
				float num2 = ManhunterPackIncidentUtility.ManhunterAnimalWeight(candidate, points);
				if (num2 == 0f)
				{
					return "0%";
				}
				return string.Format("{0}%, {1}", (num2 * 100f / num).ToString("F0"), Mathf.Max(Mathf.RoundToInt(points / candidate.combatPower), 1));
			}, string.Empty);
		}

		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, int tile, float points)
		{
			int num = Mathf.Max(Mathf.RoundToInt(points / animalKind.combatPower), 1);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < num; i++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, null, null, null, null, null, null);
				Pawn item = PawnGenerator.GeneratePawn(request);
				list.Add(item);
			}
			return list;
		}
	}
}

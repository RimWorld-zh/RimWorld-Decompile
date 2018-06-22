using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000332 RID: 818
	[HasDebugOutput]
	public static class ManhunterPackIncidentUtility
	{
		// Token: 0x06000DFA RID: 3578 RVA: 0x00077470 File Offset: 0x00075870
		public static float ManhunterAnimalWeight(PawnKindDef animal, float points)
		{
			points = Mathf.Max(points, 35f);
			float result;
			if (animal.combatPower > points)
			{
				result = 0f;
			}
			else
			{
				int num = Mathf.RoundToInt(points / animal.combatPower);
				result = Mathf.Clamp01(Mathf.InverseLerp(30f, 10f, (float)num));
			}
			return result;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x000774D0 File Offset: 0x000758D0
		public static bool TryFindManhunterAnimalKind(float points, int tile, out PawnKindDef animalKind)
		{
			return (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.canArriveManhunter && (tile == -1 || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k).TryRandomElementByWeight((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points), out animalKind);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00077524 File Offset: 0x00075924
		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, int tile, float points)
		{
			int num = Mathf.Max(Mathf.RoundToInt(points / animalKind.combatPower), 1);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < num; i++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn item = PawnGenerator.GeneratePawn(request);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000775D4 File Offset: 0x000759D4
		[DebugOutput]
		public static void ManhunterResults()
		{
			List<PawnKindDef> candidates = (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.canArriveManhunter
			orderby -k.combatPower
			select k).ToList<PawnKindDef>();
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add(20f * Mathf.Pow(1.25f, (float)i));
			}
			DebugTables.MakeTablesDialog<float, PawnKindDef>(list, (float points) => points.ToString("F0") + " pts", candidates, (PawnKindDef candidate) => candidate.defName + " (" + candidate.combatPower.ToString("F0") + ")", delegate(float points, PawnKindDef candidate)
			{
				float num = candidates.Sum((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points));
				float num2 = ManhunterPackIncidentUtility.ManhunterAnimalWeight(candidate, points);
				string result;
				if (num2 == 0f)
				{
					result = "0%";
				}
				else
				{
					result = string.Format("{0}%, {1}", (num2 * 100f / num).ToString("F0"), Mathf.Max(Mathf.RoundToInt(points / candidate.combatPower), 1));
				}
				return result;
			}, "");
		}
	}
}

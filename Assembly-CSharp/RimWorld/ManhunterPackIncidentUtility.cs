using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public static class ManhunterPackIncidentUtility
	{
		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache3;

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

		public static bool TryFindManhunterAnimalKind(float points, int tile, out PawnKindDef animalKind)
		{
			return (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.canArriveManhunter && (tile == -1 || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k).TryRandomElementByWeight((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points), out animalKind);
		}

		public static int GetAnimalsCount(PawnKindDef animalKind, float points)
		{
			return Mathf.Max(Mathf.RoundToInt(points / animalKind.combatPower), 1);
		}

		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, int tile, float points)
		{
			int animalsCount = ManhunterPackIncidentUtility.GetAnimalsCount(animalKind, points);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < animalsCount; i++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn item = PawnGenerator.GeneratePawn(request);
				list.Add(item);
			}
			return list;
		}

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

		[CompilerGenerated]
		private static bool <ManhunterResults>m__0(PawnKindDef k)
		{
			return k.RaceProps.Animal && k.canArriveManhunter;
		}

		[CompilerGenerated]
		private static float <ManhunterResults>m__1(PawnKindDef k)
		{
			return -k.combatPower;
		}

		[CompilerGenerated]
		private static string <ManhunterResults>m__2(float points)
		{
			return points.ToString("F0") + " pts";
		}

		[CompilerGenerated]
		private static string <ManhunterResults>m__3(PawnKindDef candidate)
		{
			return candidate.defName + " (" + candidate.combatPower.ToString("F0") + ")";
		}

		[CompilerGenerated]
		private sealed class <TryFindManhunterAnimalKind>c__AnonStorey0
		{
			internal int tile;

			internal float points;

			public <TryFindManhunterAnimalKind>c__AnonStorey0()
			{
			}

			internal bool <>m__0(PawnKindDef k)
			{
				return k.RaceProps.Animal && k.canArriveManhunter && (this.tile == -1 || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(this.tile, k.race));
			}

			internal float <>m__1(PawnKindDef k)
			{
				return ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, this.points);
			}
		}

		[CompilerGenerated]
		private sealed class <ManhunterResults>c__AnonStorey1
		{
			internal List<PawnKindDef> candidates;

			public <ManhunterResults>c__AnonStorey1()
			{
			}

			internal string <>m__0(float points, PawnKindDef candidate)
			{
				float num = this.candidates.Sum((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points));
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
			}

			private sealed class <ManhunterResults>c__AnonStorey2
			{
				internal float points;

				internal ManhunterPackIncidentUtility.<ManhunterResults>c__AnonStorey1 <>f__ref$1;

				public <ManhunterResults>c__AnonStorey2()
				{
				}

				internal float <>m__0(PawnKindDef k)
				{
					return ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, this.points);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ScenarioMaker
	{
		private static Scenario scen;

		public static Scenario GeneratingScenario
		{
			get
			{
				return ScenarioMaker.scen;
			}
		}

		public static Scenario GenerateNewRandomScenario(string seed)
		{
			Rand.PushState();
			Rand.Seed = seed.GetHashCode();
			int @int = Rand.Int;
			int[] array = new int[10];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Rand.Int;
			}
			int int2 = Rand.Int;
			ScenarioMaker.scen = new Scenario();
			ScenarioMaker.scen.Category = ScenarioCategory.CustomLocal;
			ScenarioMaker.scen.name = NameGenerator.GenerateName(RulePackDefOf.NamerScenario, (Predicate<string>)null, false, (string)null);
			ScenarioMaker.scen.description = (string)null;
			ScenarioMaker.scen.summary = (string)null;
			Rand.Seed = @int;
			ScenarioMaker.scen.playerFaction = (ScenPart_PlayerFaction)ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerFaction);
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.ConfigPage_ConfigureStartingPawns));
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerPawnsArriveMethod));
			Rand.Seed = array[0];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnFilter, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[1];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingImportant, Rand.RangeInclusive(0, 2));
			Rand.Seed = array[2];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnModifier, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[3];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.Rule, Rand.RangeInclusive(-2, 3));
			Rand.Seed = array[4];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingItem, Rand.RangeInclusive(0, 6));
			Rand.Seed = array[5];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.WorldThing, Rand.RangeInclusive(-3, 6));
			Rand.Seed = array[6];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.GameCondition, Rand.RangeInclusive(-1, 2));
			Rand.Seed = int2;
			foreach (ScenPart allPart in ScenarioMaker.scen.AllParts)
			{
				allPart.Randomize();
			}
			for (int j = 0; j < ScenarioMaker.scen.parts.Count; j++)
			{
				for (int k = 0; k < ScenarioMaker.scen.parts.Count; k++)
				{
					if (j != k && ScenarioMaker.scen.parts[j].TryMerge(ScenarioMaker.scen.parts[k]))
					{
						ScenarioMaker.scen.parts.RemoveAt(k);
						k--;
						if (j > k)
						{
							j--;
						}
					}
				}
			}
			for (int l = 0; l < ScenarioMaker.scen.parts.Count; l++)
			{
				for (int m = 0; m < ScenarioMaker.scen.parts.Count; m++)
				{
					if (l != m && !ScenarioMaker.scen.parts[l].CanCoexistWith(ScenarioMaker.scen.parts[m]))
					{
						ScenarioMaker.scen.parts.RemoveAt(m);
						m--;
						if (l > m)
						{
							l--;
						}
					}
				}
			}
			foreach (string item in ScenarioMaker.scen.ConfigErrors())
			{
				Log.Error(item);
			}
			Rand.PopState();
			Scenario result = ScenarioMaker.scen;
			ScenarioMaker.scen = null;
			return result;
		}

		private static void AddCategoryScenParts(Scenario scen, ScenPartCategory cat, int count)
		{
			scen.parts.AddRange(ScenarioMaker.RandomScenPartsOfCategory(scen, cat, count));
		}

		private static IEnumerable<ScenPart> RandomScenPartsOfCategory(Scenario scen, ScenPartCategory cat, int count)
		{
			_003CRandomScenPartsOfCategory_003Ec__Iterator0 _003CRandomScenPartsOfCategory_003Ec__Iterator = (_003CRandomScenPartsOfCategory_003Ec__Iterator0)/*Error near IL_0032: stateMachine*/;
			if (count > 0)
			{
				IEnumerable<ScenPartDef> allowedParts = from d in ScenarioMaker.AddableParts(scen)
				where d.category == cat
				select d;
				int numYielded = 0;
				int numTries = 0;
				while (true)
				{
					if (numYielded < count && allowedParts.Any())
					{
						ScenPartDef def = allowedParts.RandomElementByWeight((Func<ScenPartDef, float>)((ScenPartDef d) => d.selectionWeight));
						ScenPart newPart = ScenarioMaker.MakeScenPart(def);
						if (ScenarioMaker.CanAddPart(scen, newPart))
						{
							yield return newPart;
							/*Error: Unable to find new state assignment for yield return*/;
						}
						numTries++;
						if (numTries > 100)
							break;
						continue;
					}
					yield break;
				}
				Log.Error("Could not add ScenPart of category " + cat + " to scenario " + scen + " after 50 tries.");
			}
		}

		public static IEnumerable<ScenPartDef> AddableParts(Scenario scen)
		{
			return from d in DefDatabase<ScenPartDef>.AllDefs
			where scen.AllParts.Count((Func<ScenPart, bool>)((ScenPart p) => p.def == d)) < d.maxUses
			select d;
		}

		private static bool CanAddPart(Scenario scen, ScenPart newPart)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < scen.parts.Count)
				{
					if (!newPart.CanCoexistWith(scen.parts[num]))
					{
						result = false;
						break;
					}
					num++;
					continue;
				}
				result = true;
				break;
			}
			return result;
		}

		public static ScenPart MakeScenPart(ScenPartDef def)
		{
			ScenPart scenPart = (ScenPart)Activator.CreateInstance(def.scenPartClass);
			scenPart.def = def;
			return scenPart;
		}
	}
}

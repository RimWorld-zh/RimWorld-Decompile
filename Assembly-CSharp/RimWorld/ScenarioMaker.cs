using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000655 RID: 1621
	public static class ScenarioMaker
	{
		// Token: 0x0400132B RID: 4907
		private static Scenario scen;

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x0011EC3C File Offset: 0x0011D03C
		public static Scenario GeneratingScenario
		{
			get
			{
				return ScenarioMaker.scen;
			}
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x0011EC58 File Offset: 0x0011D058
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
			ScenarioMaker.scen.name = NameGenerator.GenerateName(RulePackDefOf.NamerScenario, null, false, null, null);
			ScenarioMaker.scen.description = null;
			ScenarioMaker.scen.summary = null;
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
			foreach (ScenPart scenPart in ScenarioMaker.scen.AllParts)
			{
				scenPart.Randomize();
			}
			for (int j = 0; j < ScenarioMaker.scen.parts.Count; j++)
			{
				for (int k = 0; k < ScenarioMaker.scen.parts.Count; k++)
				{
					if (j != k)
					{
						if (ScenarioMaker.scen.parts[j].TryMerge(ScenarioMaker.scen.parts[k]))
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
			}
			for (int l = 0; l < ScenarioMaker.scen.parts.Count; l++)
			{
				for (int m = 0; m < ScenarioMaker.scen.parts.Count; m++)
				{
					if (l != m)
					{
						if (!ScenarioMaker.scen.parts[l].CanCoexistWith(ScenarioMaker.scen.parts[m]))
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
			}
			foreach (string text in ScenarioMaker.scen.ConfigErrors())
			{
				Log.Error(text, false);
			}
			Rand.PopState();
			Scenario result = ScenarioMaker.scen;
			ScenarioMaker.scen = null;
			return result;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x0011F024 File Offset: 0x0011D424
		private static void AddCategoryScenParts(Scenario scen, ScenPartCategory cat, int count)
		{
			scen.parts.AddRange(ScenarioMaker.RandomScenPartsOfCategory(scen, cat, count));
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x0011F03C File Offset: 0x0011D43C
		private static IEnumerable<ScenPart> RandomScenPartsOfCategory(Scenario scen, ScenPartCategory cat, int count)
		{
			if (count <= 0)
			{
				yield break;
			}
			IEnumerable<ScenPartDef> allowedParts = from d in ScenarioMaker.AddableParts(scen)
			where d.category == cat
			select d;
			int numYielded = 0;
			int numTries = 0;
			while (numYielded < count)
			{
				if (!allowedParts.Any<ScenPartDef>())
				{
					yield break;
				}
				ScenPartDef def = allowedParts.RandomElementByWeight((ScenPartDef d) => d.selectionWeight);
				ScenPart newPart = ScenarioMaker.MakeScenPart(def);
				if (ScenarioMaker.CanAddPart(scen, newPart))
				{
					yield return newPart;
					numYielded++;
				}
				numTries++;
				if (numTries > 100)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not add ScenPart of category ",
						cat,
						" to scenario ",
						scen,
						" after 50 tries."
					}), false);
					yield break;
				}
			}
			yield break;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x0011F074 File Offset: 0x0011D474
		public static IEnumerable<ScenPartDef> AddableParts(Scenario scen)
		{
			return from d in DefDatabase<ScenPartDef>.AllDefs
			where scen.AllParts.Count((ScenPart p) => p.def == d) < d.maxUses
			select d;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x0011F0AC File Offset: 0x0011D4AC
		private static bool CanAddPart(Scenario scen, ScenPart newPart)
		{
			for (int i = 0; i < scen.parts.Count; i++)
			{
				if (!newPart.CanCoexistWith(scen.parts[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x0011F100 File Offset: 0x0011D500
		public static ScenPart MakeScenPart(ScenPartDef def)
		{
			ScenPart scenPart = (ScenPart)Activator.CreateInstance(def.scenPartClass);
			scenPart.def = def;
			return scenPart;
		}
	}
}

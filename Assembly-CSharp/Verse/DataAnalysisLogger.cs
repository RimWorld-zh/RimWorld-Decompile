using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	internal static class DataAnalysisLogger
	{
		public static void DoLog_RecipeSkills()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Recipes per skill, with work speed stats:");
			stringBuilder.AppendLine("(No skill)");
			foreach (RecipeDef current in DefDatabase<RecipeDef>.AllDefs)
			{
				if (current.workSkill == null)
				{
					stringBuilder.Append("    " + current.defName);
					if (current.workSpeedStat != null)
					{
						stringBuilder.Append(" (" + current.workSpeedStat + ")");
					}
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendLine();
			foreach (SkillDef current2 in DefDatabase<SkillDef>.AllDefs)
			{
				stringBuilder.AppendLine(current2.label);
				foreach (RecipeDef current3 in DefDatabase<RecipeDef>.AllDefs)
				{
					if (current3.workSkill == current2)
					{
						stringBuilder.Append("    " + current3.defName);
						if (current3.workSpeedStat != null)
						{
							stringBuilder.Append(" (" + current3.workSpeedStat);
							if (!current3.workSpeedStat.skillNeedFactors.NullOrEmpty<SkillNeed>())
							{
								stringBuilder.Append(" - " + GenText.ToCommaList(from fac in current3.workSpeedStat.skillNeedFactors
								select fac.skill.defName, false));
							}
							stringBuilder.Append(")");
						}
						stringBuilder.AppendLine();
					}
				}
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString().TrimEndNewlines());
		}

		public static void DoLog_RaidStrategies()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Raid strategy chances:");
			float num = (from d in DefDatabase<RaidStrategyDef>.AllDefs
			select d.Worker.SelectionChance(Find.VisibleMap)).Sum();
			foreach (RaidStrategyDef current in DefDatabase<RaidStrategyDef>.AllDefs)
			{
				float num2 = current.Worker.SelectionChance(Find.VisibleMap);
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					current.defName,
					": ",
					num2.ToString("F2"),
					" (",
					(num2 / num).ToStringPercent(),
					")"
				}));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_StockGeneratorsDefs()
		{
			if (Find.VisibleMap == null)
			{
				Log.Error("Requires visible map.");
				return;
			}
			StringBuilder sb = new StringBuilder();
			Action<StockGenerator> action = delegate(StockGenerator gen)
			{
				sb.AppendLine(gen.GetType().ToString());
				sb.AppendLine("ALLOWED DEFS:");
				foreach (ThingDef current in from d in DefDatabase<ThingDef>.AllDefs
				where gen.HandlesThingDef(d)
				select d)
				{
					sb.AppendLine(string.Concat(new object[]
					{
						current.defName,
						" [",
						current.BaseMarketValue,
						"]"
					}));
				}
				sb.AppendLine();
				sb.AppendLine("GENERATION TEST:");
				gen.countRange = IntRange.one;
				for (int i = 0; i < 30; i++)
				{
					foreach (Thing current2 in gen.GenerateThings(Find.VisibleMap.Tile))
					{
						sb.AppendLine(string.Concat(new object[]
						{
							current2.Label,
							" [",
							current2.MarketValue,
							"]"
						}));
					}
				}
				sb.AppendLine("---------------------------------------------------------");
			};
			action(new StockGenerator_Armor());
			action(new StockGenerator_WeaponsRanged());
			action(new StockGenerator_Clothes());
			action(new StockGenerator_Art());
			Log.Message(sb.ToString());
		}

		public static void DoLog_TraderStockMarketValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TraderKindDef current in DefDatabase<TraderKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(current.defName + " : " + ((ItemCollectionGenerator_TraderStock)ItemCollectionGeneratorDefOf.TraderStock.Worker).AverageTotalStockValue(current).ToString("F0"));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_TraderStockGeneration()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TraderKindDef current in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localDef = current;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate
				{
					Log.Message(((ItemCollectionGenerator_TraderStock)ItemCollectionGeneratorDefOf.TraderStock.Worker).GenerationDataFor(localDef));
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_BodyPartTagGroups()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef current in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = current;
				FloatMenuOption item = new FloatMenuOption(localBd.defName, delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(localBd.defName + "\n----------------");
					foreach (string tag in (from elem in localBd.AllParts.SelectMany((BodyPartRecord part) => part.def.tags)
					orderby elem
					select elem).Distinct<string>())
					{
						stringBuilder.AppendLine(tag);
						foreach (BodyPartRecord current2 in from part in localBd.AllParts
						where part.def.tags.Contains(tag)
						orderby part.def.defName
						select part)
						{
							stringBuilder.AppendLine("  " + current2.def.defName);
						}
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_LoadedAssets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Mesh));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Meshes: ",
				array.Length,
				" (",
				DataAnalysisLogger.TotalBytes(array).ToStringBytes("F2"),
				")"
			}));
			UnityEngine.Object[] array2 = Resources.FindObjectsOfTypeAll(typeof(Material));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Materials: ",
				array2.Length,
				" (",
				DataAnalysisLogger.TotalBytes(array2).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   Damaged: " + DamagedMatPool.MatCount);
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   Faded: ",
				FadedMaterialPool.TotalMaterialCount,
				" (",
				FadedMaterialPool.TotalMaterialBytes.ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   SolidColorsSimple: " + SolidColorMaterials.SimpleColorMatCount);
			UnityEngine.Object[] array3 = Resources.FindObjectsOfTypeAll(typeof(Texture));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Textures: ",
				array3.Length,
				" (",
				DataAnalysisLogger.TotalBytes(array3).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Texture list:");
			UnityEngine.Object[] array4 = array3;
			for (int i = 0; i < array4.Length; i++)
			{
				UnityEngine.Object @object = array4[i];
				string text = ((Texture)@object).name;
				if (text.NullOrEmpty())
				{
					text = "-";
				}
				stringBuilder.AppendLine(text);
			}
			Log.Message(stringBuilder.ToString());
		}

		private static int TotalBytes(UnityEngine.Object[] arr)
		{
			int num = 0;
			for (int i = 0; i < arr.Length; i++)
			{
				UnityEngine.Object o = arr[i];
				num += Profiler.GetRuntimeMemorySize(o);
			}
			return num;
		}

		public static void DoLog_MinifiableTags()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
			{
				if (current.Minifiable)
				{
					stringBuilder.Append(current.defName);
					if (!current.tradeTags.NullOrEmpty<string>())
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(GenText.ToCommaList(current.tradeTags, true));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_ItemDeteriorationRates()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef current in from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && !d.destroyOnDrop && d.useHitPoints
			orderby d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) descending
			select d)
			{
				stringBuilder.AppendLine(current.defName + "  " + current.GetStatValueAbstract(StatDefOf.DeteriorationRate, null).ToString("F1"));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_ItemBeauties()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef current in from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && !d.destroyOnDrop
			orderby d.GetStatValueAbstract(StatDefOf.Beauty, null) descending
			select d)
			{
				stringBuilder.AppendLine(current.defName + "  " + current.GetStatValueAbstract(StatDefOf.Beauty, null).ToString("F1"));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_TestRulepack()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (RulePackDef current in DefDatabase<RulePackDef>.AllDefs)
			{
				RulePackDef localNamer = current;
				FloatMenuOption item = new FloatMenuOption(localNamer.defName, delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 200; i++)
					{
						stringBuilder.AppendLine(NameGenerator.GenerateName(localNamer, null, false));
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_GeneratedNames()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (RulePackDef current in DefDatabase<RulePackDef>.AllDefs)
			{
				RulePackDef localRp = current;
				FloatMenuOption item = new FloatMenuOption(localRp.defName, delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Test names for " + localRp.defName + ":");
					for (int i = 0; i < 200; i++)
					{
						stringBuilder.AppendLine(NameGenerator.GenerateName(localRp, null, false));
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_ThingList()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(ThingRequestGroup)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingRequestGroup localRg2 = (ThingRequestGroup)((byte)enumerator.Current);
					ThingRequestGroup localRg = localRg2;
					FloatMenuOption item = new FloatMenuOption(localRg.ToString(), delegate
					{
						StringBuilder stringBuilder = new StringBuilder();
						List<Thing> list2 = Find.VisibleMap.listerThings.ThingsInGroup(localRg);
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"Global things in group ",
							localRg,
							" (count ",
							list2.Count,
							")"
						}));
						Log.Message(DebugLogsUtility.ThingListToUniqueCountString(list2));
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_SimpleCurveTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			SimpleCurve simpleCurve = new SimpleCurve
			{
				{
					new CurvePoint(5f, 0f),
					true
				},
				{
					new CurvePoint(10f, 1f),
					true
				},
				{
					new CurvePoint(20f, 3f),
					true
				},
				{
					new CurvePoint(40f, 2f),
					true
				}
			};
			for (int i = 0; i < 50; i++)
			{
				stringBuilder.AppendLine(i + " -> " + simpleCurve.Evaluate((float)i));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_TestPawnNames()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("------Testing parsing");
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John 'Nick' Smith"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John 'Nick' von Smith"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John Smith"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John von Smith"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John 'Nick Hell' Smith"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John 'Nick Hell' von Smith"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John Nick Hell von Smith"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John O'Neil"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John 'O'Neil"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John 'O'Farley' Neil"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("John 'O'''Farley' Neil"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("O'Shea 'O'Farley' O'Neil"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("Missing 'Lastname'"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("Missing 'Lastnamewithspace'     "));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("Unbalanc'ed 'De'limiters'     "));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult("\t"));
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult(string.Empty));
			stringBuilder.AppendLine("------Testing ResolveMissingPieces consistency");
			for (int i = 0; i < 20; i++)
			{
				NameTriple nameTriple = new NameTriple("John", null, "Last");
				nameTriple.ResolveMissingPieces(null);
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					nameTriple.ToString(),
					"       [",
					nameTriple.First,
					"] [",
					nameTriple.Nick,
					"] [",
					nameTriple.Last,
					"]"
				}));
			}
			Log.Message(stringBuilder.ToString());
		}

		private static string PawnNameTestResult(string rawName)
		{
			NameTriple nameTriple = NameTriple.FromString(rawName);
			nameTriple.ResolveMissingPieces(null);
			return string.Concat(new string[]
			{
				rawName,
				" -> ",
				nameTriple.ToString(),
				"       [",
				nameTriple.First,
				"] [",
				nameTriple.Nick,
				"] [",
				nameTriple.Last,
				"]"
			});
		}

		public static void DoLog_PassabilityFill()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
			{
				if (current.passability != Traversability.Standable || current.fillPercent > 0f)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						current.defName,
						" - pass=",
						current.passability.ToString(),
						", fill=",
						current.fillPercent.ToStringPercent()
					}));
					if (current.passability == Traversability.Impassable && current.fillPercent < 0.1f)
					{
						stringBuilder.Append("   ALERT, impassable with low fill");
					}
					if (current.passability != Traversability.Impassable && current.fillPercent > 0.8f)
					{
						stringBuilder.Append("    ALERT, passabile with very high fill");
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_AnimalsPerBiome()
		{
			IEnumerable<BiomeDef> enumerable = from d in DefDatabase<BiomeDef>.AllDefs
			where d.animalDensity > 0f
			select d;
			IOrderedEnumerable<PawnKindDef> orderedEnumerable = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race.race.Animal
			orderby (!d.race.race.predator) ? 0 : 1
			select d;
			string text = string.Empty;
			text += "name      commonality     commonalityShare\n\n";
			foreach (BiomeDef b in enumerable)
			{
				float num = orderedEnumerable.Sum((PawnKindDef a) => b.CommonalityOfAnimal(a));
				float f = (from a in orderedEnumerable
				where a.race.race.predator
				select a).Sum((PawnKindDef a) => b.CommonalityOfAnimal(a)) / num;
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					b.label,
					"   (predators: ",
					f.ToStringPercent("F2"),
					")"
				});
				foreach (PawnKindDef current in orderedEnumerable)
				{
					float num2 = b.CommonalityOfAnimal(current);
					if (num2 > 0f)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"\n    ",
							current.label,
							"       ",
							num2.ToString("F3"),
							"       ",
							(num2 / num).ToStringPercent("F2")
						});
					}
				}
				text += "\n\n";
			}
			Log.Message(text);
		}

		public static void DoLog_SmeltProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
			{
				Thing thing = ThingMaker.MakeThing(current, GenStuff.DefaultStuffFor(current));
				if (thing.SmeltProducts(1f).Any<Thing>())
				{
					stringBuilder.Append(thing.LabelCap + ": ");
					foreach (Thing current2 in thing.SmeltProducts(1f))
					{
						stringBuilder.Append(" " + current2.Label);
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_PawnArrivalCandidates()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(IncidentDefOf.RaidEnemy.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidEnemy.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.RaidFriendly.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.RaidFriendly.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.VisitorGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.VisitorGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TravelerGroup.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TravelerGroup.Worker).DebugListingOfGroupSources());
			stringBuilder.AppendLine(IncidentDefOf.TraderCaravanArrival.defName);
			stringBuilder.AppendLine(((IncidentWorker_PawnsArrive)IncidentDefOf.TraderCaravanArrival.Worker).DebugListingOfGroupSources());
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_SpecificTaleDescs()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TaleDef current in DefDatabase<TaleDef>.AllDefs)
			{
				TaleDef localDef = current;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, delegate
				{
					TaleTester.LogSpecificTale(localDef, 40);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_SocialPropernessMatters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Social-properness-matters things:");
			foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
			{
				if (current.socialPropernessMatters)
				{
					stringBuilder.AppendLine(string.Format("  {0}", current.defName));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_FoodPreferability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Food, ordered by preferability:");
			foreach (ThingDef current in from td in DefDatabase<ThingDef>.AllDefs
			where td.ingestible != null
			orderby td.ingestible.preferability
			select td)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", current.ingestible.preferability, current.defName));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_CaravanRequests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Caravan request sample:");
			Map visibleMap = Find.VisibleMap;
			IncidentWorker_CaravanRequest incidentWorker_CaravanRequest = (IncidentWorker_CaravanRequest)IncidentDefOf.CaravanRequest.Worker;
			for (int i = 0; i < 100; i++)
			{
				Settlement settlement = IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(visibleMap.Tile);
				if (settlement == null)
				{
					break;
				}
				CaravanRequestComp component = settlement.GetComponent<CaravanRequestComp>();
				incidentWorker_CaravanRequest.GenerateCaravanRequest(component, visibleMap);
				stringBuilder.AppendLine(string.Format("  {0} -> {1}", GenLabel.ThingLabel(component.requestThingDef, null, component.requestCount), component.rewards[0].Label, ThingDefOf.Silver.label));
				settlement.GetComponent<CaravanRequestComp>().Disable();
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}

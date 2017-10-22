using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.AI;
using Verse.Profile;
using Verse.Steam;

namespace Verse
{
	internal static class DataAnalysisLogger
	{
		public static void DoLog_QualityGenData()
		{
			QualityUtility.LogGenerationData();
		}

		public static void DoLog_SongSelectionInfo()
		{
			Find.MusicManagerPlay.LogSongSelectionData();
		}

		public static void DoLog_PlantData()
		{
			GenPlant.LogPlantData();
		}

		public static void DoLog_AgeInjuries()
		{
			AgeInjuryUtility.LogOldInjuryCalculations();
		}

		public static void DoLog_PawnGroupsMade()
		{
			PawnGroupMakerUtility.LogPawnGroupsMade();
		}

		public static void DoLog_AllGraphicsInDatabase()
		{
			GraphicDatabase.DebugLogAllGraphics();
		}

		public static void DoLog_RandTests()
		{
			Rand.LogRandTests();
		}

		public static void DoLog_SteamWorkshopWtatus()
		{
			Workshop.LogStatus();
		}

		public static void DoLog_MathPerf()
		{
			GenMath.LogTestMathPerf();
		}

		public static void DoLog_MeshPoolStats()
		{
			MeshPool.LogStats();
		}

		public static void DoLog_Lords()
		{
			Find.VisibleMap.lordManager.LogLords();
		}

		public static void DoLog_PodContentsTest()
		{
			IncidentWorker_ResourcePodCrash.DebugLogPodContentsChoices();
		}

		public static void DoLog_PodContentsPossible()
		{
			IncidentWorker_ResourcePodCrash.DebugLogPossiblePodContentsDefs();
		}

		public static void DoLog_PathCostIgnoreRepeaters()
		{
			PathGrid.LogPathCostIgnoreRepeaters();
		}

		public static void DoLog_AnimalStockGen()
		{
			StockGenerator_Animals.LogStockGeneration();
		}

		public static void DoLog_ObjectsLoaded()
		{
			MemoryTracker.LogObjectsLoaded();
		}

		public static void DoLog_ObjectHoldPaths()
		{
			MemoryTracker.LogObjectHoldPaths();
		}

		public static void DoLog_ListSolidBackstories()
		{
			IEnumerable<string> enumerable = SolidBioDatabase.allBios.SelectMany((Func<PawnBio, IEnumerable<string>>)((PawnBio bio) => bio.adulthood.spawnCategories)).Distinct();
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (string item2 in enumerable)
			{
				string catInner = item2;
				FloatMenuOption item = new FloatMenuOption(catInner, (Action)delegate()
				{
					IEnumerable<PawnBio> enumerable2 = from b in SolidBioDatabase.allBios
					where b.adulthood.spawnCategories.Contains(catInner)
					select b;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Backstories with category: " + catInner + " (" + enumerable2.Count() + ")");
					foreach (PawnBio item in enumerable2)
					{
						stringBuilder.AppendLine(item.ToString());
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_KeyStrings()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int value in Enum.GetValues(typeof(KeyCode)))
			{
				stringBuilder.AppendLine(((Enum)(object)(KeyCode)value).ToString() + " - " + ((KeyCode)value).ToStringReadable());
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_PawnKindGear()
		{
			IOrderedEnumerable<PawnKindDef> orderedEnumerable = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			orderby k.combatPower
			select k;
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef item2 in orderedEnumerable)
			{
				Faction fac = FactionUtility.DefaultFactionFrom(item2.defaultFactionType);
				PawnKindDef kind = item2;
				FloatMenuOption item = new FloatMenuOption(kind.defName + "(" + kind.combatPower + ", $" + kind.weaponMoney + ")", (Action)delegate()
				{
					DefMap<ThingDef, int> weapons = new DefMap<ThingDef, int>();
					for (int i = 0; i < 500; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(kind, fac);
						if (pawn.equipment.Primary != null)
						{
							DefMap<ThingDef, int> defMap;
							DefMap<ThingDef, int> obj = defMap = weapons;
							ThingDef def;
							ThingDef def2 = def = pawn.equipment.Primary.def;
							int num = defMap[def];
							obj[def2] = num + 1;
						}
						pawn.Destroy(DestroyMode.Vanish);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Weapons spawned from " + 500 + "x " + kind.defName);
					foreach (ThingDef item in from t in DefDatabase<ThingDef>.AllDefs
					orderby weapons[t] descending
					select t)
					{
						int num2 = weapons[item];
						if (num2 > 0)
						{
							stringBuilder.AppendLine("  " + item.defName + "    " + ((float)((float)num2 / 500.0)).ToStringPercent());
						}
					}
					Log.Message(stringBuilder.ToString().TrimEndNewlines());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_RecipeSkills()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Recipes per skill, with work speed stats:");
			stringBuilder.AppendLine("(No skill)");
			foreach (RecipeDef allDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (allDef.workSkill == null)
				{
					stringBuilder.Append("    " + allDef.defName);
					if (allDef.workSpeedStat != null)
					{
						stringBuilder.Append(" (" + allDef.workSpeedStat + ")");
					}
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendLine();
			foreach (SkillDef allDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				stringBuilder.AppendLine(allDef2.label);
				foreach (RecipeDef allDef3 in DefDatabase<RecipeDef>.AllDefs)
				{
					if (allDef3.workSkill == allDef2)
					{
						stringBuilder.Append("    " + allDef3.defName);
						if (allDef3.workSpeedStat != null)
						{
							stringBuilder.Append(" (" + allDef3.workSpeedStat);
							if (!allDef3.workSpeedStat.skillNeedFactors.NullOrEmpty())
							{
								stringBuilder.Append(" - " + GenText.ToCommaList(from fac in allDef3.workSpeedStat.skillNeedFactors
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
			foreach (RaidStrategyDef allDef in DefDatabase<RaidStrategyDef>.AllDefs)
			{
				float num2 = allDef.Worker.SelectionChance(Find.VisibleMap);
				stringBuilder.AppendLine(allDef.defName + ": " + num2.ToString("F2") + " (" + (num2 / num).ToStringPercent() + ")");
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_StockGeneratorsDefs()
		{
			if (Find.VisibleMap == null)
			{
				Log.Error("Requires visible map.");
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				Action<StockGenerator> action = (Action<StockGenerator>)delegate(StockGenerator gen)
				{
					sb.AppendLine(gen.GetType().ToString());
					sb.AppendLine("ALLOWED DEFS:");
					foreach (ThingDef item in from d in DefDatabase<ThingDef>.AllDefs
					where gen.HandlesThingDef(d)
					select d)
					{
						sb.AppendLine(item.defName + " [" + item.BaseMarketValue + "]");
					}
					sb.AppendLine();
					sb.AppendLine("GENERATION TEST:");
					gen.countRange = IntRange.one;
					for (int i = 0; i < 30; i++)
					{
						foreach (Thing item2 in gen.GenerateThings(Find.VisibleMap.Tile))
						{
							sb.AppendLine(item2.Label + " [" + item2.MarketValue + "]");
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
		}

		public static void DoLog_TraderStockMarketValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TraderKindDef allDef in DefDatabase<TraderKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(allDef.defName + " : " + ((ItemCollectionGenerator_TraderStock)ItemCollectionGeneratorDefOf.TraderStock.Worker).AverageTotalStockValue(allDef).ToString("F0"));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_TraderStockGeneration()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (TraderKindDef allDef in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localDef = allDef;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, (Action)delegate
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
			foreach (BodyDef allDef in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = allDef;
				FloatMenuOption item = new FloatMenuOption(localBd.defName, (Action)delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(localBd.defName + "\n----------------");
					using (IEnumerator<string> enumerator2 = (from elem in localBd.AllParts.SelectMany((Func<BodyPartRecord, IEnumerable<string>>)((BodyPartRecord part) => part.def.tags))
					orderby elem
					select elem).Distinct().GetEnumerator())
					{
						string tag;
						while (enumerator2.MoveNext())
						{
							tag = enumerator2.Current;
							stringBuilder.AppendLine(tag);
							foreach (BodyPartRecord item in from part in localBd.AllParts
							where part.def.tags.Contains(tag)
							orderby part.def.defName
							select part)
							{
								stringBuilder.AppendLine("  " + item.def.defName);
							}
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
			stringBuilder.AppendLine("Meshes: " + array.Length + " (" + DataAnalysisLogger.TotalBytes(array).ToStringBytes("F2") + ")");
			UnityEngine.Object[] array2 = Resources.FindObjectsOfTypeAll(typeof(Material));
			stringBuilder.AppendLine("Materials: " + array2.Length + " (" + DataAnalysisLogger.TotalBytes(array2).ToStringBytes("F2") + ")");
			stringBuilder.AppendLine("   Damaged: " + DamagedMatPool.MatCount);
			stringBuilder.AppendLine("   Faded: " + FadedMaterialPool.TotalMaterialCount + " (" + FadedMaterialPool.TotalMaterialBytes.ToStringBytes("F2") + ")");
			stringBuilder.AppendLine("   SolidColorsSimple: " + SolidColorMaterials.SimpleColorMatCount);
			UnityEngine.Object[] array3 = Resources.FindObjectsOfTypeAll(typeof(Texture));
			stringBuilder.AppendLine("Textures: " + array3.Length + " (" + DataAnalysisLogger.TotalBytes(array3).ToStringBytes("F2") + ")");
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
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.Minifiable)
				{
					stringBuilder.Append(allDef.defName);
					if (!allDef.tradeTags.NullOrEmpty())
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(GenText.ToCommaList(allDef.tradeTags, true));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_ItemBeauties()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef item in from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && !d.destroyOnDrop
			orderby d.GetStatValueAbstract(StatDefOf.Beauty, null) descending
			select d)
			{
				stringBuilder.AppendLine(item.defName + "  " + item.GetStatValueAbstract(StatDefOf.Beauty, null).ToString("F1"));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_TestRulepack()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (RulePackDef allDef in DefDatabase<RulePackDef>.AllDefs)
			{
				RulePackDef localNamer = allDef;
				FloatMenuOption item = new FloatMenuOption(localNamer.defName, (Action)delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 200; i++)
					{
						stringBuilder.AppendLine(NameGenerator.GenerateName(localNamer, (Predicate<string>)null, false));
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
			foreach (RulePackDef allDef in DefDatabase<RulePackDef>.AllDefs)
			{
				RulePackDef localRp = allDef;
				FloatMenuOption item = new FloatMenuOption(localRp.defName, (Action)delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Test names for " + localRp.defName + ":");
					for (int i = 0; i < 200; i++)
					{
						stringBuilder.AppendLine(NameGenerator.GenerateName(localRp, (Predicate<string>)null, false));
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
			foreach (byte value in Enum.GetValues(typeof(ThingRequestGroup)))
			{
				ThingRequestGroup localRg = (ThingRequestGroup)value;
				FloatMenuOption item = new FloatMenuOption(((Enum)(object)localRg).ToString(), (Action)delegate
				{
					StringBuilder stringBuilder = new StringBuilder();
					List<Thing> list2 = Find.VisibleMap.listerThings.ThingsInGroup(localRg);
					stringBuilder.AppendLine("Global things in group " + localRg + " (count " + list2.Count + ")");
					Log.Message(DebugLogsUtility.ThingListToUniqueCountString(list2));
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static void DoLog_SimpleCurveTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			SimpleCurve simpleCurve = new SimpleCurve();
			simpleCurve.Add(new CurvePoint(5f, 0f), true);
			simpleCurve.Add(new CurvePoint(10f, 1f), true);
			simpleCurve.Add(new CurvePoint(20f, 3f), true);
			simpleCurve.Add(new CurvePoint(40f, 2f), true);
			SimpleCurve simpleCurve2 = simpleCurve;
			for (int i = 0; i < 50; i++)
			{
				stringBuilder.AppendLine(i + " -> " + simpleCurve2.Evaluate((float)i));
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
				NameTriple nameTriple = new NameTriple("John", (string)null, "Last");
				nameTriple.ResolveMissingPieces((string)null);
				stringBuilder.AppendLine(nameTriple.ToString() + "       [" + nameTriple.First + "] [" + nameTriple.Nick + "] [" + nameTriple.Last + "]");
			}
			Log.Message(stringBuilder.ToString());
		}

		private static string PawnNameTestResult(string rawName)
		{
			NameTriple nameTriple = NameTriple.FromString(rawName);
			nameTriple.ResolveMissingPieces((string)null);
			return rawName + " -> " + nameTriple.ToString() + "       [" + nameTriple.First + "] [" + nameTriple.Nick + "] [" + nameTriple.Last + "]";
		}

		public static void DoLog_PassabilityFill()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.passability != 0 || allDef.fillPercent > 0.0)
				{
					stringBuilder.Append(allDef.defName + " - pass=" + ((Enum)(object)allDef.passability).ToString() + ", fill=" + allDef.fillPercent.ToStringPercent());
					if (allDef.passability == Traversability.Impassable && allDef.fillPercent < 0.10000000149011612)
					{
						stringBuilder.Append("   ALERT, impassable with low fill");
					}
					if (allDef.passability != Traversability.Impassable && allDef.fillPercent > 0.800000011920929)
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
			where d.animalDensity > 0.0
			select d;
			IOrderedEnumerable<PawnKindDef> source = from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race.race.Animal
			orderby d.race.race.predator ? 1 : 0
			select d;
			string empty = string.Empty;
			empty += "name      commonality     commonalityShare     size\n\n";
			using (IEnumerator<BiomeDef> enumerator = enumerable.GetEnumerator())
			{
				BiomeDef b;
				while (enumerator.MoveNext())
				{
					b = enumerator.Current;
					float num = source.Sum((Func<PawnKindDef, float>)((PawnKindDef a) => b.CommonalityOfAnimal(a)));
					float f = (from a in source
					where a.race.race.predator
					select a).Sum((Func<PawnKindDef, float>)((PawnKindDef a) => b.CommonalityOfAnimal(a))) / num;
					float num2 = source.Sum((Func<PawnKindDef, float>)((PawnKindDef a) => b.CommonalityOfAnimal(a) * a.race.race.baseBodySize));
					float f2 = (from a in source
					where a.race.race.predator
					select a).Sum((Func<PawnKindDef, float>)((PawnKindDef a) => b.CommonalityOfAnimal(a) * a.race.race.baseBodySize)) / num2;
					string text = empty;
					empty = text + b.label + "   (predators: " + f.ToStringPercent("F2") + ", predators by size: " + f2.ToStringPercent("F2") + ")";
					foreach (PawnKindDef item in from a in source
					orderby b.CommonalityOfAnimal(a) descending
					select a)
					{
						float num3 = b.CommonalityOfAnimal(item);
						if (num3 > 0.0)
						{
							text = empty;
							empty = text + "\n    " + item.label + ((!item.RaceProps.predator) ? string.Empty : "*") + "       " + num3.ToString("F3") + "       " + (num3 / num).ToStringPercent("F2") + "       " + item.race.race.baseBodySize.ToString("F2");
						}
					}
					empty += "\n\n";
				}
			}
			Log.Message(empty);
		}

		public static void DoLog_SmeltProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				Thing thing = ThingMaker.MakeThing(allDef, GenStuff.DefaultStuffFor(allDef));
				if (thing.SmeltProducts(1f).Any())
				{
					stringBuilder.Append(thing.LabelCap + ": ");
					foreach (Thing item in thing.SmeltProducts(1f))
					{
						stringBuilder.Append(" " + item.Label);
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
			foreach (TaleDef allDef in DefDatabase<TaleDef>.AllDefs)
			{
				TaleDef localDef = allDef;
				FloatMenuOption item = new FloatMenuOption(localDef.defName, (Action)delegate
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
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.socialPropernessMatters)
				{
					stringBuilder.AppendLine(string.Format("  {0}", allDef.defName));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_FoodPreferability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Food, ordered by preferability:");
			foreach (ThingDef item in from td in DefDatabase<ThingDef>.AllDefs
			where td.ingestible != null
			orderby td.ingestible.preferability
			select td)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", item.ingestible.preferability, item.defName));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_CaravanRequests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Caravan request sample:");
			Map visibleMap = Find.VisibleMap;
			IncidentWorker_CaravanRequest incidentWorker_CaravanRequest = (IncidentWorker_CaravanRequest)IncidentDefOf.CaravanRequest.Worker;
			int num = 0;
			while (num < 100)
			{
				Settlement settlement = IncidentWorker_CaravanRequest.RandomNearbyTradeableSettlement(visibleMap.Tile);
				if (settlement != null)
				{
					CaravanRequestComp component = ((WorldObject)settlement).GetComponent<CaravanRequestComp>();
					incidentWorker_CaravanRequest.GenerateCaravanRequest(component, visibleMap);
					stringBuilder.AppendLine(string.Format("  {0} -> {1}", GenLabel.ThingLabel(component.requestThingDef, null, component.requestCount), component.rewards[0].Label, ThingDefOf.Silver.label));
					((WorldObject)settlement).GetComponent<CaravanRequestComp>().Disable();
					num++;
					continue;
				}
				break;
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}

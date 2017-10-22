using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
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

		public static void DoLog_SteamWorkshopStatus()
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
			ItemCollectionGenerator_ResourcePod.DebugLogPodContentsChoices();
		}

		public static void DoLog_PodContentsPossible()
		{
			ItemCollectionGenerator_ResourcePod.DebugLogPossiblePodContentsDefs();
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

		public static void DoLog_GrownCollections_Start()
		{
			CollectionsTracker.RememberCollections();
		}

		public static void DoLog_GrownCollections_End()
		{
			CollectionsTracker.LogGrownCollections();
		}

		public static void DoLog_PeaceTalksChances()
		{
			PeaceTalks.LogChances();
		}

		public static void DoLog_DamageTest()
		{
			ThingDef thingDef = ThingDef.Named("Bullet_BoltActionRifle");
			PawnKindDef pawnKindDef = PawnKindDef.Named("Slave");
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			DamageInfo dinfo = new DamageInfo(thingDef.projectile.damageDef, thingDef.projectile.damageAmountBase, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
			int num = 0;
			int num2 = 0;
			DefMap<BodyPartDef, int> defMap = new DefMap<BodyPartDef, int>();
			for (int i = 0; i < 500; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, faction);
				List<BodyPartDef> list = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList();
				for (int j = 0; j < 2; j++)
				{
					pawn.TakeDamage(dinfo);
					if (pawn.Dead)
					{
						num++;
						break;
					}
				}
				List<BodyPartDef> list2 = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList();
				if (list2.Count > list.Count)
				{
					num2++;
					foreach (BodyPartDef item in list2)
					{
						DefMap<BodyPartDef, int> defMap2;
						BodyPartDef def;
						(defMap2 = defMap)[def = item] = defMap2[def] + 1;
					}
					foreach (BodyPartDef item2 in list)
					{
						DefMap<BodyPartDef, int> defMap2;
						BodyPartDef def2;
						(defMap2 = defMap)[def2 = item2] = defMap2[def2] - 1;
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Damage test");
			stringBuilder.AppendLine("Hit " + 500 + " " + pawnKindDef.label + "s with " + 2 + "x " + thingDef.label + " (" + thingDef.projectile.damageAmountBase + " damage) each. Results:");
			stringBuilder.AppendLine("Killed: " + num + " / " + 500 + " (" + ((float)((float)num / 500.0)).ToStringPercent() + ")");
			stringBuilder.AppendLine("Part losers: " + num2 + " / " + 500 + " (" + ((float)((float)num2 / 500.0)).ToStringPercent() + ")");
			stringBuilder.AppendLine("Parts lost:");
			foreach (BodyPartDef allDef in DefDatabase<BodyPartDef>.AllDefs)
			{
				if (defMap[allDef] > 0)
				{
					stringBuilder.AppendLine("   " + allDef.label + ": " + defMap[allDef]);
				}
			}
			Log.Message(stringBuilder.ToString());
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

		public static void DoLog_WorkDisables()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef item2 in from ki in DefDatabase<PawnKindDef>.AllDefs
			where ki.RaceProps.Humanlike
			select ki)
			{
				PawnKindDef pkInner = item2;
				Faction faction = FactionUtility.DefaultFactionFrom(pkInner.defaultFactionType);
				FloatMenuOption item = new FloatMenuOption(pkInner.defName, (Action)delegate()
				{
					int num = 500;
					DefMap<WorkTypeDef, int> defMap = new DefMap<WorkTypeDef, int>();
					for (int num2 = 0; num2 < num; num2++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(pkInner, faction);
						foreach (WorkTypeDef disabledWorkType in pawn.story.DisabledWorkTypes)
						{
							DefMap<WorkTypeDef, int> defMap2;
							WorkTypeDef def;
							(defMap2 = defMap)[def = disabledWorkType] = defMap2[def] + 1;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Generated " + num + " pawns of kind " + pkInner.defName + " on faction " + faction);
					stringBuilder.AppendLine("Work types disabled:");
					foreach (WorkTypeDef allDef in DefDatabase<WorkTypeDef>.AllDefs)
					{
						if (allDef.workTags != 0)
						{
							stringBuilder.AppendLine("   " + allDef.defName + ": " + defMap[allDef] + "        " + ((float)defMap[allDef] / (float)num).ToStringPercent());
						}
					}
					IEnumerable<Backstory> enumerable = BackstoryDatabase.allBackstories.Select((Func<KeyValuePair<string, Backstory>, Backstory>)((KeyValuePair<string, Backstory> kvp) => kvp.Value));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTypeDef disable rates (there are " + enumerable.Count() + " backstories):");
					foreach (WorkTypeDef allDef2 in DefDatabase<WorkTypeDef>.AllDefs)
					{
						int num3 = 0;
						foreach (Backstory item in enumerable)
						{
							if (item.DisabledWorkTypes.Any((Func<WorkTypeDef, bool>)((WorkTypeDef wd) => allDef2 == wd)))
							{
								num3++;
							}
						}
						stringBuilder.AppendLine("   " + allDef2.defName + ": " + num3 + "     " + ((float)num3 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent());
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTag disable rates (there are " + enumerable.Count() + " backstories):");
					IEnumerator enumerator6 = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
					try
					{
						while (enumerator6.MoveNext())
						{
							WorkTags workTags = (WorkTags)enumerator6.Current;
							int num4 = 0;
							foreach (Backstory item2 in enumerable)
							{
								if ((workTags & item2.workDisables) != 0)
								{
									num4++;
								}
							}
							stringBuilder.AppendLine("   " + workTags + ": " + num4 + "     " + ((float)num4 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent());
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator6 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
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
			IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyCode k = (KeyCode)enumerator.Current;
					stringBuilder.AppendLine(k.ToString() + " - " + k.ToStringReadable());
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
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
							ThingDef def;
							(defMap = weapons)[def = pawn.equipment.Primary.def] = defMap[def] + 1;
						}
						pawn.Destroy(DestroyMode.Vanish);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Weapons spawned from " + 500 + "x " + kind.defName);
					foreach (ThingDef item in from t in DefDatabase<ThingDef>.AllDefs
					orderby weapons[t] descending
					select t)
					{
						int num = weapons[item];
						if (num > 0)
						{
							stringBuilder.AppendLine("  " + item.defName + "    " + ((float)((float)num / 500.0)).ToStringPercent());
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
				stringBuilder.AppendLine(allDef2.LabelCap);
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

		public static void DoLog_ItemCollectionGeneration()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ItemCollectionGeneratorDef allDef in DefDatabase<ItemCollectionGeneratorDef>.AllDefs)
			{
				ItemCollectionGeneratorDef localDef = allDef;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, (Action)delegate()
				{
					Action<ItemCollectionGeneratorParams> generate = (Action<ItemCollectionGeneratorParams>)delegate(ItemCollectionGeneratorParams parms)
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < 50; i++)
						{
							List<Thing> list4 = localDef.Worker.Generate(parms);
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
							}
							float num = 0f;
							for (int j = 0; j < list4.Count; j++)
							{
								stringBuilder.AppendLine("   - " + list4[j].LabelCap);
								num += list4[j].MarketValue * (float)list4[j].stackCount;
								list4[j].Destroy(DestroyMode.Vanish);
							}
							stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
						}
						Log.Message(stringBuilder.ToString());
					};
					if (localDef == ItemCollectionGeneratorDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction allFaction in Find.FactionManager.AllFactions)
						{
							if (allFaction != Faction.OfPlayer)
							{
								Faction localF = allFaction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, (Action)delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef item in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = item;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, (Action)delegate
										{
											ItemCollectionGeneratorParams obj = new ItemCollectionGeneratorParams
											{
												traderFaction = localF,
												traderDef = localKind
											};
											generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}
					else
					{
						generate(default(ItemCollectionGeneratorParams));
					}
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
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
					foreach (string item in (from elem in localBd.AllParts.SelectMany((Func<BodyPartRecord, IEnumerable<string>>)((BodyPartRecord part) => part.def.tags))
					orderby elem
					select elem).Distinct())
					{
						stringBuilder.AppendLine(item);
						foreach (BodyPartRecord item2 in from part in localBd.AllParts
						where part.def.tags.Contains(item)
						orderby part.def.defName
						select part)
						{
							stringBuilder.AppendLine("  " + item2.def.defName);
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

		private static long TotalBytes(UnityEngine.Object[] arr)
		{
			long num = 0L;
			for (int i = 0; i < arr.Length; i++)
			{
				UnityEngine.Object o = arr[i];
				num += Profiler.GetRuntimeMemorySizeLong(o);
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
						stringBuilder.AppendLine(NameGenerator.GenerateName(localNamer, (Predicate<string>)null, false, (string)null));
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
						stringBuilder.AppendLine(NameGenerator.GenerateName(localRp, (Predicate<string>)null, false, (string)null));
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		private static void CreateDamagedDestroyedMenu(Action<Action<List<BodyPartDef>, List<bool>>> callback)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<int> damagedes = Enumerable.Range(0, 5);
			IEnumerable<int> destroyedes = Enumerable.Range(0, 5);
			foreach (Pair<int, int> item2 in damagedes.Concat(-1).Cross(destroyedes.Concat(-1)))
			{
				DebugMenuOption item = new DebugMenuOption(string.Format("{0} damaged/{1} destroyed", (item2.First != -1) ? item2.First.ToString() : "(random)", (item2.Second != -1) ? item2.Second.ToString() : "(random)"), DebugMenuOptionMode.Action, (Action)delegate()
				{
					callback((Action<List<BodyPartDef>, List<bool>>)delegate(List<BodyPartDef> bodyparts, List<bool> flags)
					{
						Pair<int, int> damageddestroyed2;
						int num = damageddestroyed2.First;
						int destroyed = damageddestroyed2.Second;
						if (num == -1)
						{
							num = damagedes.RandomElement();
						}
						if (destroyed == -1)
						{
							destroyed = destroyedes.RandomElement();
						}
						Pair<BodyPartDef, bool>[] source = (from idx in Enumerable.Range(0, num + destroyed)
						select new Pair<BodyPartDef, bool>(DefDatabase<BodyPartDef>.AllDefsListForReading.RandomElement(), idx < destroyed)).InRandomOrder(null).ToArray();
						bodyparts.Clear();
						flags.Clear();
						bodyparts.AddRange(from part in source
						select part.First);
						flags.AddRange(from part in source
						select part.Second);
					});
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		public static void DoLog_FlavorfulCombatTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<ManeuverDef> maneuvers = DefDatabase<ManeuverDef>.AllDefsListForReading;
			IEnumerable<RulePackDef> results = new RulePackDef[3]
			{
				RulePackDefOf.Combat_Hit,
				RulePackDefOf.Combat_Miss,
				RulePackDefOf.Combat_Dodge
			};
			foreach (Pair<ManeuverDef, RulePackDef> item2 in maneuvers.Concat(null).Cross(results.Concat(null)))
			{
				DebugMenuOption item = new DebugMenuOption(string.Format("{0}/{1}", (item2.First != null) ? item2.First.defName : "(random)", (item2.Second != null) ? item2.Second.defName : "(random)"), DebugMenuOptionMode.Action, (Action)delegate()
				{
					DataAnalysisLogger.CreateDamagedDestroyedMenu((Action<Action<List<BodyPartDef>, List<bool>>>)delegate(Action<List<BodyPartDef>, List<bool>> bodyPartCreator)
					{
						StringBuilder stringBuilder5 = new StringBuilder();
						for (int m = 0; m < 100; m++)
						{
							Pair<ManeuverDef, RulePackDef> maneuverresult2;
							ManeuverDef maneuver = maneuverresult2.First;
							RulePackDef rulePackDef = maneuverresult2.Second;
							if (maneuver == null)
							{
								maneuver = maneuvers.RandomElement();
							}
							if (rulePackDef == null)
							{
								rulePackDef = results.RandomElement();
							}
							List<BodyPartDef> list6 = null;
							List<bool> list7 = null;
							if (rulePackDef == RulePackDefOf.Combat_Hit)
							{
								list6 = new List<BodyPartDef>();
								list7 = new List<bool>();
								bodyPartCreator(list6, list7);
							}
							Pair<ThingDef, Tool> pair = (from ttp in (from td in DefDatabase<ThingDef>.AllDefsListForReading
							where td.IsMeleeWeapon && !td.tools.NullOrEmpty()
							select td).SelectMany((Func<ThingDef, IEnumerable<Pair<ThingDef, Tool>>>)((ThingDef td) => from tool in td.tools
							select new Pair<ThingDef, Tool>(td, tool)))
							where ttp.Second.capacities.Contains(maneuver.requiredCapacity)
							select ttp).RandomElement();
							BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackDef, maneuver.combatLogRules, CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), ImplementOwnerTypeDefOf.Weapon, pair.Second.label, pair.First, null);
							battleLogEntry_MeleeCombat.FillTargets(list6, list7);
							battleLogEntry_MeleeCombat.Debug_OverrideTicks(Rand.Int);
							stringBuilder5.AppendLine(battleLogEntry_MeleeCombat.ToGameStringFromPOV(null));
						}
						Log.Message(stringBuilder5.ToString());
					});
				});
				list.Add(item);
			}
			list.Add(new DebugMenuOption("Ranged fire", DebugMenuOptionMode.Action, (Action)delegate()
			{
				StringBuilder stringBuilder4 = new StringBuilder();
				for (int l = 0; l < 100; l++)
				{
					ThingDef thingDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon
					select td).RandomElement();
					bool flag = Rand.Value < 0.20000000298023224;
					bool flag2 = !flag && Rand.Value < 0.949999988079071;
					BattleLogEntry_RangedFire battleLogEntry_RangedFire = new BattleLogEntry_RangedFire(CombatLogTester.GenerateRandom(), (!flag) ? CombatLogTester.GenerateRandom() : null, (!flag2) ? thingDef : null, null);
					battleLogEntry_RangedFire.Debug_OverrideTicks(Rand.Int);
					stringBuilder4.AppendLine(battleLogEntry_RangedFire.ToGameStringFromPOV(null));
				}
				Log.Message(stringBuilder4.ToString());
			}));
			list.Add(new DebugMenuOption("Ranged impact hit", DebugMenuOptionMode.Action, (Action)delegate()
			{
				DataAnalysisLogger.CreateDamagedDestroyedMenu((Action<Action<List<BodyPartDef>, List<bool>>>)delegate(Action<List<BodyPartDef>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					for (int k = 0; k < 100; k++)
					{
						ThingDef weaponDef3 = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon
						select td).RandomElement();
						List<BodyPartDef> list4 = new List<BodyPartDef>();
						List<bool> list5 = new List<bool>();
						bodyPartCreator(list4, list5);
						Pawn pawn = CombatLogTester.GenerateRandom();
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact3 = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), pawn, pawn, weaponDef3, null);
						battleLogEntry_RangedImpact3.FillTargets(list4, list5);
						battleLogEntry_RangedImpact3.Debug_OverrideTicks(Rand.Int);
						stringBuilder3.AppendLine(battleLogEntry_RangedImpact3.ToGameStringFromPOV(null));
					}
					Log.Message(stringBuilder3.ToString());
				});
			}));
			list.Add(new DebugMenuOption("Ranged impact miss", DebugMenuOptionMode.Action, (Action)delegate()
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int j = 0; j < 100; j++)
				{
					ThingDef weaponDef2 = (from td in DefDatabase<ThingDef>.AllDefsListForReading
					where td.IsRangedWeapon
					select td).RandomElement();
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact2 = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), null, CombatLogTester.GenerateRandom(), weaponDef2, null);
					battleLogEntry_RangedImpact2.Debug_OverrideTicks(Rand.Int);
					stringBuilder2.AppendLine(battleLogEntry_RangedImpact2.ToGameStringFromPOV(null));
				}
				Log.Message(stringBuilder2.ToString());
			}));
			list.Add(new DebugMenuOption("Ranged impact hit incorrect", DebugMenuOptionMode.Action, (Action)delegate()
			{
				DataAnalysisLogger.CreateDamagedDestroyedMenu((Action<Action<List<BodyPartDef>, List<bool>>>)delegate(Action<List<BodyPartDef>, List<bool>> bodyPartCreator)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < 100; i++)
					{
						ThingDef weaponDef = (from td in DefDatabase<ThingDef>.AllDefsListForReading
						where td.IsRangedWeapon
						select td).RandomElement();
						List<BodyPartDef> list2 = new List<BodyPartDef>();
						List<bool> list3 = new List<bool>();
						bodyPartCreator(list2, list3);
						BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), CombatLogTester.GenerateRandom(), weaponDef, null);
						battleLogEntry_RangedImpact.FillTargets(list2, list3);
						battleLogEntry_RangedImpact.Debug_OverrideTicks(Rand.Int);
						stringBuilder.AppendLine(battleLogEntry_RangedImpact.ToGameStringFromPOV(null));
					}
					Log.Message(stringBuilder.ToString());
				});
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		public static void DoLog_ThingList()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			IEnumerator enumerator = Enum.GetValues(typeof(ThingRequestGroup)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ThingRequestGroup thingRequestGroup = (ThingRequestGroup)enumerator.Current;
					ThingRequestGroup localRg = thingRequestGroup;
					FloatMenuOption item = new FloatMenuOption(localRg.ToString(), (Action)delegate
					{
						StringBuilder stringBuilder = new StringBuilder();
						List<Thing> list2 = Find.VisibleMap.listerThings.ThingsInGroup(localRg);
						stringBuilder.AppendLine("Global things in group " + localRg + " (count " + list2.Count + ")");
						Log.Message(DebugLogsUtility.ThingListToUniqueCountString(list2));
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
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
			stringBuilder.AppendLine(DataAnalysisLogger.PawnNameTestResult(""));
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
					stringBuilder.Append(allDef.defName + " - pass=" + allDef.passability.ToString() + ", fill=" + allDef.fillPercent.ToStringPercent());
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
			string str = "";
			str += "name      commonality     commonalityShare     size\n\n";
			foreach (BiomeDef item in enumerable)
			{
				float num = source.Sum((Func<PawnKindDef, float>)((PawnKindDef a) => item.CommonalityOfAnimal(a)));
				float f = (from a in source
				where a.race.race.predator
				select a).Sum((Func<PawnKindDef, float>)((PawnKindDef a) => item.CommonalityOfAnimal(a))) / num;
				float num2 = source.Sum((Func<PawnKindDef, float>)((PawnKindDef a) => item.CommonalityOfAnimal(a) * a.race.race.baseBodySize));
				float f2 = (from a in source
				where a.race.race.predator
				select a).Sum((Func<PawnKindDef, float>)((PawnKindDef a) => item.CommonalityOfAnimal(a) * a.race.race.baseBodySize)) / num2;
				string text = str;
				str = text + item.label + "   (predators: " + f.ToStringPercent("F2") + ", predators by size: " + f2.ToStringPercent("F2") + ")";
				foreach (PawnKindDef item2 in from a in source
				orderby item.CommonalityOfAnimal(a) descending
				select a)
				{
					float num3 = item.CommonalityOfAnimal(item2);
					if (num3 > 0.0)
					{
						text = str;
						str = text + "\n    " + item2.label + ((!item2.RaceProps.predator) ? "" : "*") + "       " + num3.ToString("F3") + "       " + (num3 / num).ToStringPercent("F2") + "       " + item2.race.race.baseBodySize.ToString("F2");
					}
				}
				str += "\n\n";
			}
			Log.Message(str);
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
			foreach (TaleDef item2 in from def in DefDatabase<TaleDef>.AllDefs
			orderby def.defName
			select def)
			{
				TaleDef localDef = item2;
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

		public static void DoLog_MapDanger()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Map danger status:");
			foreach (Map map in Find.Maps)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", map, map.dangerWatcher.DangerRating));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_OreValues()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Ore values:");
			foreach (ThingDef item in from def in DefDatabase<ThingDef>.AllDefsListForReading
			where def.mineable && def.building != null && def.building.mineableThing != null
			orderby def.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)def.building.mineableYield
			select def)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", item.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)item.building.mineableYield, item));
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DoLog_ItemWorkTime()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Item work time:");
			Func<ThingDef, float> workToBuild = (Func<ThingDef, float>)((ThingDef def) => Mathf.Max(def.GetStatValueAbstract(StatDefOf.WorkToMake, null), def.GetStatValueAbstract(StatDefOf.WorkToBuild, null)));
			foreach (ThingDef item in (from def in DefDatabase<ThingDef>.AllDefsListForReading
			where workToBuild(def) > 1.0
			select def).OrderBy(workToBuild))
			{
				stringBuilder.AppendLine(string.Format("{0} {1}: {2}", workToBuild(item), (item.building == null) ? "" : " B", item));
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}

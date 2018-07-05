using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public static class PawnApparelGenerator
	{
		private static List<ThingStuffPair> allApparelPairs = new List<ThingStuffPair>();

		private static float freeWarmParkaMaxPrice;

		private static float freeWarmHatMaxPrice;

		private static PawnApparelGenerator.PossibleApparelSet workingSet = new PawnApparelGenerator.PossibleApparelSet();

		private static List<ThingStuffPair> usableApparel = new List<ThingStuffPair>();

		private static StringBuilder debugSb = null;

		[CompilerGenerated]
		private static Predicate<ThingDef> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingStuffPair, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingStuffPair, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<ThingStuffPair, string> <>f__am$cacheA;

		static PawnApparelGenerator()
		{
			PawnApparelGenerator.Reset();
		}

		public static void Reset()
		{
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((ThingDef td) => td.IsApparel);
			PawnApparelGenerator.freeWarmParkaMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Parka, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeWarmHatMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Tuque, ThingDefOf.Cloth) * 1.3f));
		}

		public static void GenerateStartingApparelFor(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.RaceProps.ToolUser && pawn.RaceProps.IsFlesh)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
				float randomInRange = pawn.kindDef.apparelMoney.RandomInRange;
				float mapTemperature;
				NeededWarmth neededWarmth = PawnApparelGenerator.ApparelWarmthNeededNow(pawn, request, out mapTemperature);
				bool flag = Rand.Value < pawn.kindDef.apparelAllowHeadgearChance;
				PawnApparelGenerator.debugSb = null;
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb = new StringBuilder();
					PawnApparelGenerator.debugSb.AppendLine("Generating apparel for " + pawn);
					PawnApparelGenerator.debugSb.AppendLine("Money: " + randomInRange.ToString("F0"));
					PawnApparelGenerator.debugSb.AppendLine("Needed warmth: " + neededWarmth);
					PawnApparelGenerator.debugSb.AppendLine("Headgear allowed: " + flag);
				}
				if (randomInRange >= 0.001f)
				{
					int num = 0;
					for (;;)
					{
						PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, flag);
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.Append(num.ToString().PadRight(5) + "Trying: " + PawnApparelGenerator.workingSet.ToString());
						}
						if (num >= 10 || Rand.Value >= 0.85f)
						{
							goto IL_1EA;
						}
						float num2 = Rand.Range(0.45f, 0.8f);
						float totalPrice = PawnApparelGenerator.workingSet.TotalPrice;
						if (totalPrice >= randomInRange * num2)
						{
							goto IL_1EA;
						}
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine(string.Concat(new string[]
							{
								" -- Failed: Spent $",
								totalPrice.ToString("F0"),
								", < ",
								(num2 * 100f).ToString("F0"),
								"% of money."
							}));
						}
						IL_367:
						num++;
						continue;
						IL_1EA:
						if (num < 20 && Rand.Value < 0.97f)
						{
							if (!PawnApparelGenerator.workingSet.Covers(BodyPartGroupDefOf.Torso))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Does not cover torso.");
								}
								goto IL_367;
							}
						}
						if (num < 30 && Rand.Value < 0.8f)
						{
							if (PawnApparelGenerator.workingSet.CoatButNoShirt())
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Coat but no shirt.");
								}
								goto IL_367;
							}
						}
						if (num < 50)
						{
							bool mustBeSafe = num < 17;
							if (!PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, mustBeSafe, mapTemperature))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Wrong warmth.");
								}
								goto IL_367;
							}
						}
						if (num < 80 && PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
						{
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Naked.");
							}
							goto IL_367;
						}
						break;
					}
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.Append(string.Concat(new object[]
						{
							" -- Approved! Total price: $",
							PawnApparelGenerator.workingSet.TotalPrice.ToString("F0"),
							", TotalInsulationCold: ",
							PawnApparelGenerator.workingSet.TotalInsulationCold
						}));
					}
				}
				if ((!pawn.kindDef.apparelIgnoreSeasons || request.ForceAddFreeWarmLayerIfNeeded) && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, true, mapTemperature))
				{
					PawnApparelGenerator.workingSet.AddFreeWarmthAsNeeded(neededWarmth, mapTemperature);
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					Log.Message(PawnApparelGenerator.debugSb.ToString(), false);
				}
				PawnApparelGenerator.workingSet.GiveToPawn(pawn);
				PawnApparelGenerator.workingSet.Reset(null, null);
			}
		}

		private static void GenerateWorkingPossibleApparelSetFor(Pawn pawn, float money, bool headwearAllowed)
		{
			PawnApparelGenerator.workingSet.Reset(pawn.RaceProps.body, pawn.def);
			float num = money;
			List<ThingDef> reqApparel = pawn.kindDef.apparelRequired;
			if (reqApparel != null)
			{
				int i;
				for (i = 0; i < reqApparel.Count; i++)
				{
					ThingStuffPair pair = (from pa in PawnApparelGenerator.allApparelPairs
					where pa.thing == reqApparel[i]
					select pa).RandomElementByWeight((ThingStuffPair pa) => pa.Commonality);
					PawnApparelGenerator.workingSet.Add(pair);
					num -= pair.Price;
				}
			}
			int @int = Rand.Int;
			while (Rand.Value >= 0.1f)
			{
				PawnApparelGenerator.usableApparel.Clear();
				for (int j = 0; j < PawnApparelGenerator.allApparelPairs.Count; j++)
				{
					ThingStuffPair thingStuffPair = PawnApparelGenerator.allApparelPairs[j];
					if (PawnApparelGenerator.CanUsePair(thingStuffPair, pawn, num, headwearAllowed, @int))
					{
						PawnApparelGenerator.usableApparel.Add(thingStuffPair);
					}
				}
				ThingStuffPair pair2;
				bool flag = PawnApparelGenerator.usableApparel.TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair2);
				PawnApparelGenerator.usableApparel.Clear();
				if (!flag)
				{
					return;
				}
				PawnApparelGenerator.workingSet.Add(pair2);
				num -= pair2.Price;
			}
		}

		private static bool CanUsePair(ThingStuffPair pair, Pawn pawn, float moneyLeft, bool allowHeadgear, int fixedSeed)
		{
			bool result;
			if (pair.Price > moneyLeft)
			{
				result = false;
			}
			else if (!allowHeadgear && PawnApparelGenerator.IsHeadgear(pair.thing))
			{
				result = false;
			}
			else if (pair.stuff != null && pawn.Faction != null && !pawn.Faction.def.CanUseStuffForApparel(pair.stuff))
			{
				result = false;
			}
			else if (PawnApparelGenerator.workingSet.PairOverlapsAnything(pair))
			{
				result = false;
			}
			else
			{
				if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
				{
					bool flag = false;
					for (int i = 0; i < pawn.kindDef.apparelTags.Count; i++)
					{
						for (int j = 0; j < pair.thing.apparel.tags.Count; j++)
						{
							if (pawn.kindDef.apparelTags[i] == pair.thing.apparel.tags[j])
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (pair.thing.generateAllowChance < 1f)
				{
					if (!Rand.ChanceSeeded(pair.thing.generateAllowChance, fixedSeed ^ (int)pair.thing.shortHash ^ 64128343))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public static bool IsHeadgear(ThingDef td)
		{
			return td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead);
		}

		private static NeededWarmth ApparelWarmthNeededNow(Pawn pawn, PawnGenerationRequest request, out float mapTemperature)
		{
			int tile = request.Tile;
			if (tile == -1)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			NeededWarmth result;
			if (tile == -1)
			{
				mapTemperature = 21f;
				result = NeededWarmth.Any;
			}
			else
			{
				NeededWarmth neededWarmth = NeededWarmth.Any;
				Twelfth twelfth = GenLocalDate.Twelfth(tile);
				mapTemperature = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
				for (int i = 0; i < 2; i++)
				{
					NeededWarmth neededWarmth2 = PawnApparelGenerator.CalculateNeededWarmth(pawn, tile, twelfth);
					if (neededWarmth2 != NeededWarmth.Any)
					{
						neededWarmth = neededWarmth2;
						break;
					}
					twelfth = twelfth.NextTwelfth();
				}
				if (pawn.kindDef.apparelIgnoreSeasons)
				{
					if (request.ForceAddFreeWarmLayerIfNeeded && neededWarmth == NeededWarmth.Warm)
					{
						result = neededWarmth;
					}
					else
					{
						result = NeededWarmth.Any;
					}
				}
				else
				{
					result = neededWarmth;
				}
			}
			return result;
		}

		public static NeededWarmth CalculateNeededWarmth(Pawn pawn, int tile, Twelfth twelfth)
		{
			float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			NeededWarmth result;
			if (num < pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 4f)
			{
				result = NeededWarmth.Warm;
			}
			else if (num > pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) + 4f)
			{
				result = NeededWarmth.Cool;
			}
			else
			{
				result = NeededWarmth.Any;
			}
			return result;
		}

		[DebugOutput]
		private static void ApparelPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnApparelGenerator.allApparelPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[7];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", (ThingStuffPair p) => (p.stuff == null) ? "" : p.stuff.defName);
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => (p.Commonality * 100f).ToString("F4"));
			array[4] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F4"));
			array[5] = new TableDataGetter<ThingStuffPair>("insulationCold", (ThingStuffPair p) => (p.InsulationCold != 0f) ? p.InsulationCold.ToString() : "");
			array[6] = new TableDataGetter<ThingStuffPair>("headgear", (ThingStuffPair p) => (!PawnApparelGenerator.IsHeadgear(p.thing)) ? "" : "*");
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		[DebugOutput]
		private static void ApparelPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnApparelGenerator.allApparelPairs);
		}

		[CompilerGenerated]
		private static bool <Reset>m__0(ThingDef td)
		{
			return td.IsApparel;
		}

		[CompilerGenerated]
		private static float <GenerateWorkingPossibleApparelSetFor>m__1(ThingStuffPair pa)
		{
			return pa.Commonality;
		}

		[CompilerGenerated]
		private static float <GenerateWorkingPossibleApparelSetFor>m__2(ThingStuffPair pa)
		{
			return pa.Commonality;
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__3(ThingStuffPair p)
		{
			return p.thing.defName;
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__4(ThingStuffPair p)
		{
			return p.thing.defName;
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__5(ThingStuffPair p)
		{
			return (p.stuff == null) ? "" : p.stuff.defName;
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__6(ThingStuffPair p)
		{
			return p.Price.ToString();
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__7(ThingStuffPair p)
		{
			return (p.Commonality * 100f).ToString("F4");
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__8(ThingStuffPair p)
		{
			return p.thing.generateCommonality.ToString("F4");
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__9(ThingStuffPair p)
		{
			return (p.InsulationCold != 0f) ? p.InsulationCold.ToString() : "";
		}

		[CompilerGenerated]
		private static string <ApparelPairs>m__A(ThingStuffPair p)
		{
			return (!PawnApparelGenerator.IsHeadgear(p.thing)) ? "" : "*";
		}

		private class PossibleApparelSet
		{
			private List<ThingStuffPair> aps = new List<ThingStuffPair>();

			private HashSet<ApparelUtility.LayerGroupPair> lgps = new HashSet<ApparelUtility.LayerGroupPair>();

			private BodyDef body;

			private ThingDef raceDef;

			private const float StartingMinTemperature = 12f;

			private const float TargetMinTemperature = -40f;

			private const float StartingMaxTemperature = 32f;

			private const float TargetMaxTemperature = 30f;

			[CompilerGenerated]
			private static Func<ThingStuffPair, float> <>f__am$cache0;

			[CompilerGenerated]
			private static Func<ThingStuffPair, float> <>f__am$cache1;

			[CompilerGenerated]
			private static Func<ThingStuffPair, float> <>f__am$cache2;

			[CompilerGenerated]
			private static Func<ThingStuffPair, float> <>f__am$cache3;

			[CompilerGenerated]
			private static Func<ThingStuffPair, float> <>f__am$cache4;

			[CompilerGenerated]
			private static Func<ThingStuffPair, float> <>f__am$cache5;

			public PossibleApparelSet()
			{
			}

			public int Count
			{
				get
				{
					return this.aps.Count;
				}
			}

			public float TotalPrice
			{
				get
				{
					return this.aps.Sum((ThingStuffPair pa) => pa.Price);
				}
			}

			public float TotalInsulationCold
			{
				get
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
				}
			}

			public void Reset(BodyDef body, ThingDef raceDef)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.body = body;
				this.raceDef = raceDef;
			}

			public void Add(ThingStuffPair pair)
			{
				this.aps.Add(pair);
				ApparelUtility.GenerateLayerGroupPairs(this.body, pair.thing, delegate(ApparelUtility.LayerGroupPair lgp)
				{
					this.lgps.Add(lgp);
				});
			}

			public bool PairOverlapsAnything(ThingStuffPair pair)
			{
				bool conflicts = false;
				ApparelUtility.GenerateLayerGroupPairs(this.body, pair.thing, delegate(ApparelUtility.LayerGroupPair lgp)
				{
					conflicts |= this.lgps.Contains(lgp);
				});
				return conflicts;
			}

			public bool CoatButNoShirt()
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						for (int j = 0; j < this.aps[i].thing.apparel.layers.Count; j++)
						{
							ApparelLayerDef apparelLayerDef = this.aps[i].thing.apparel.layers[j];
							if (apparelLayerDef == ApparelLayerDefOf.OnSkin)
							{
								flag2 = true;
							}
							if (apparelLayerDef == ApparelLayerDefOf.Shell || apparelLayerDef == ApparelLayerDefOf.Middle)
							{
								flag = true;
							}
						}
					}
				}
				return flag && !flag2;
			}

			public bool Covers(BodyPartGroupDef bp)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(bp))
					{
						return true;
					}
				}
				return false;
			}

			public bool IsNaked(Gender gender)
			{
				bool result;
				if (gender != Gender.Male)
				{
					if (gender != Gender.Female)
					{
						result = (gender != Gender.None && false);
					}
					else
					{
						result = (!this.Covers(BodyPartGroupDefOf.Legs) || !this.Covers(BodyPartGroupDefOf.Torso));
					}
				}
				else
				{
					result = !this.Covers(BodyPartGroupDefOf.Legs);
				}
				return result;
			}

			public bool SatisfiesNeededWarmth(NeededWarmth warmth, bool mustBeSafe = false, float mapTemperature = 21f)
			{
				bool result;
				if (warmth == NeededWarmth.Any)
				{
					result = true;
				}
				else if (mustBeSafe && !GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(mapTemperature))
				{
					result = false;
				}
				else if (warmth == NeededWarmth.Cool)
				{
					float num = this.aps.Sum((ThingStuffPair a) => a.InsulationHeat);
					result = (num >= -2f);
				}
				else
				{
					if (warmth != NeededWarmth.Warm)
					{
						throw new NotImplementedException();
					}
					float num2 = this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
					result = (num2 >= 52f);
				}
				return result;
			}

			public void AddFreeWarmthAsNeeded(NeededWarmth warmth, float mapTemperature)
			{
				if (warmth != NeededWarmth.Any)
				{
					if (warmth != NeededWarmth.Cool)
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine();
							PawnApparelGenerator.debugSb.AppendLine("Trying to give free warm layer.");
						}
						for (int i = 0; i < 3; i++)
						{
							if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine("Checking to give free torso-cover at max price " + PawnApparelGenerator.freeWarmParkaMaxPrice);
								}
								Predicate<ThingStuffPair> parkaPairValidator = delegate(ThingStuffPair pa)
								{
									bool result;
									if (pa.Price > PawnApparelGenerator.freeWarmParkaMaxPrice)
									{
										result = false;
									}
									else if (pa.InsulationCold <= 0f)
									{
										result = false;
									}
									else if (!pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
									{
										result = false;
									}
									else
									{
										float replacedInsulationCold = this.GetReplacedInsulationCold(pa);
										result = (replacedInsulationCold < pa.InsulationCold);
									}
									return result;
								};
								int j = 0;
								while (j < 2)
								{
									ThingStuffPair candidate;
									if (j == 0)
									{
										if ((from pa in PawnApparelGenerator.allApparelPairs
										where parkaPairValidator(pa) && pa.InsulationCold < 40f
										select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out candidate))
										{
											goto IL_142;
										}
									}
									else if ((from pa in PawnApparelGenerator.allApparelPairs
									where parkaPairValidator(pa)
									select pa).TryMaxBy((ThingStuffPair x) => x.InsulationCold - this.GetReplacedInsulationCold(x), out candidate))
									{
										goto IL_142;
									}
									j++;
									continue;
									IL_142:
									if (DebugViewSettings.logApparelGeneration)
									{
										PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
										{
											"Giving free torso-cover: ",
											candidate,
											" insulation=",
											candidate.InsulationCold
										}));
										foreach (ThingStuffPair thingStuffPair in from a in this.aps
										where !ApparelUtility.CanWearTogether(a.thing, candidate.thing, this.body)
										select a)
										{
											PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
											{
												"    -replaces ",
												thingStuffPair.ToString(),
												" InsulationCold=",
												thingStuffPair.InsulationCold
											}));
										}
									}
									this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, candidate.thing, this.body));
									this.aps.Add(candidate);
									break;
								}
							}
							if (GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(mapTemperature))
							{
								break;
							}
						}
						if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
						{
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.AppendLine("Checking to give free hat at max price " + PawnApparelGenerator.freeWarmHatMaxPrice);
							}
							Predicate<ThingStuffPair> hatPairValidator = delegate(ThingStuffPair pa)
							{
								bool result;
								if (pa.Price > PawnApparelGenerator.freeWarmHatMaxPrice)
								{
									result = false;
								}
								else if (pa.InsulationCold < 7f)
								{
									result = false;
								}
								else if (!pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) && !pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
								{
									result = false;
								}
								else
								{
									float replacedInsulationCold = this.GetReplacedInsulationCold(pa);
									result = (replacedInsulationCold < pa.InsulationCold);
								}
								return result;
							};
							ThingStuffPair hatPair;
							if ((from pa in PawnApparelGenerator.allApparelPairs
							where hatPairValidator(pa)
							select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out hatPair))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
									{
										"Giving free hat: ",
										hatPair,
										" insulation=",
										hatPair.InsulationCold
									}));
									foreach (ThingStuffPair thingStuffPair2 in from a in this.aps
									where !ApparelUtility.CanWearTogether(a.thing, hatPair.thing, this.body)
									select a)
									{
										PawnApparelGenerator.debugSb.AppendLine(string.Concat(new object[]
										{
											"    -replaces ",
											thingStuffPair2.ToString(),
											" InsulationCold=",
											thingStuffPair2.InsulationCold
										}));
									}
								}
								this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, hatPair.thing, this.body));
								this.aps.Add(hatPair);
							}
						}
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine("New TotalInsulationCold: " + this.TotalInsulationCold);
						}
					}
				}
			}

			public void GiveToPawn(Pawn pawn)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					Apparel apparel = (Apparel)ThingMaker.MakeThing(this.aps[i].thing, this.aps[i].stuff);
					PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
					if (ApparelUtility.HasPartsToWear(pawn, apparel.def))
					{
						pawn.apparel.Wear(apparel, false);
					}
				}
				for (int j = 0; j < this.aps.Count; j++)
				{
					for (int k = 0; k < this.aps.Count; k++)
					{
						if (j != k && !ApparelUtility.CanWearTogether(this.aps[j].thing, this.aps[k].thing, pawn.RaceProps.body))
						{
							Log.Error(string.Concat(new object[]
							{
								pawn,
								" generated with apparel that cannot be worn together: ",
								this.aps[j],
								", ",
								this.aps[k]
							}), false);
							return;
						}
					}
				}
			}

			private float GetReplacedInsulationCold(ThingStuffPair newAp)
			{
				float num = 0f;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(this.aps[i].thing, newAp.thing, this.body))
					{
						num += this.aps[i].InsulationCold;
					}
				}
				return num;
			}

			public override string ToString()
			{
				string str = "[";
				for (int i = 0; i < this.aps.Count; i++)
				{
					str = str + this.aps[i].ToString() + ", ";
				}
				return str + "]";
			}

			[CompilerGenerated]
			private static float <get_TotalPrice>m__0(ThingStuffPair pa)
			{
				return pa.Price;
			}

			[CompilerGenerated]
			private static float <get_TotalInsulationCold>m__1(ThingStuffPair a)
			{
				return a.InsulationCold;
			}

			[CompilerGenerated]
			private void <Add>m__2(ApparelUtility.LayerGroupPair lgp)
			{
				this.lgps.Add(lgp);
			}

			[CompilerGenerated]
			private static float <SatisfiesNeededWarmth>m__3(ThingStuffPair a)
			{
				return a.InsulationHeat;
			}

			[CompilerGenerated]
			private static float <SatisfiesNeededWarmth>m__4(ThingStuffPair a)
			{
				return a.InsulationCold;
			}

			[CompilerGenerated]
			private static float <AddFreeWarmthAsNeeded>m__5(ThingStuffPair pa)
			{
				return pa.Commonality / (pa.Price * pa.Price);
			}

			[CompilerGenerated]
			private static float <AddFreeWarmthAsNeeded>m__6(ThingStuffPair pa)
			{
				return pa.Commonality / (pa.Price * pa.Price);
			}

			[CompilerGenerated]
			private sealed class <PairOverlapsAnything>c__AnonStorey0
			{
				internal bool conflicts;

				internal PawnApparelGenerator.PossibleApparelSet $this;

				public <PairOverlapsAnything>c__AnonStorey0()
				{
				}

				internal void <>m__0(ApparelUtility.LayerGroupPair lgp)
				{
					this.conflicts |= this.$this.lgps.Contains(lgp);
				}
			}

			[CompilerGenerated]
			private sealed class <AddFreeWarmthAsNeeded>c__AnonStorey1
			{
				internal Predicate<ThingStuffPair> parkaPairValidator;

				internal PawnApparelGenerator.PossibleApparelSet $this;

				public <AddFreeWarmthAsNeeded>c__AnonStorey1()
				{
				}

				internal bool <>m__0(ThingStuffPair pa)
				{
					bool result;
					if (pa.Price > PawnApparelGenerator.freeWarmParkaMaxPrice)
					{
						result = false;
					}
					else if (pa.InsulationCold <= 0f)
					{
						result = false;
					}
					else if (!pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						result = false;
					}
					else
					{
						float replacedInsulationCold = this.$this.GetReplacedInsulationCold(pa);
						result = (replacedInsulationCold < pa.InsulationCold);
					}
					return result;
				}
			}

			[CompilerGenerated]
			private sealed class <AddFreeWarmthAsNeeded>c__AnonStorey2
			{
				internal ThingStuffPair candidate;

				internal PawnApparelGenerator.PossibleApparelSet.<AddFreeWarmthAsNeeded>c__AnonStorey1 <>f__ref$1;

				public <AddFreeWarmthAsNeeded>c__AnonStorey2()
				{
				}

				internal bool <>m__0(ThingStuffPair pa)
				{
					return this.<>f__ref$1.parkaPairValidator(pa) && pa.InsulationCold < 40f;
				}

				internal bool <>m__1(ThingStuffPair pa)
				{
					return this.<>f__ref$1.parkaPairValidator(pa);
				}

				internal float <>m__2(ThingStuffPair x)
				{
					return x.InsulationCold - this.<>f__ref$1.$this.GetReplacedInsulationCold(x);
				}

				internal bool <>m__3(ThingStuffPair a)
				{
					return !ApparelUtility.CanWearTogether(a.thing, this.candidate.thing, this.<>f__ref$1.$this.body);
				}

				internal bool <>m__4(ThingStuffPair pa)
				{
					return !ApparelUtility.CanWearTogether(pa.thing, this.candidate.thing, this.<>f__ref$1.$this.body);
				}
			}

			[CompilerGenerated]
			private sealed class <AddFreeWarmthAsNeeded>c__AnonStorey3
			{
				internal Predicate<ThingStuffPair> hatPairValidator;

				internal ThingStuffPair hatPair;

				internal PawnApparelGenerator.PossibleApparelSet $this;

				public <AddFreeWarmthAsNeeded>c__AnonStorey3()
				{
				}

				internal bool <>m__0(ThingStuffPair pa)
				{
					bool result;
					if (pa.Price > PawnApparelGenerator.freeWarmHatMaxPrice)
					{
						result = false;
					}
					else if (pa.InsulationCold < 7f)
					{
						result = false;
					}
					else if (!pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) && !pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
					{
						result = false;
					}
					else
					{
						float replacedInsulationCold = this.$this.GetReplacedInsulationCold(pa);
						result = (replacedInsulationCold < pa.InsulationCold);
					}
					return result;
				}

				internal bool <>m__1(ThingStuffPair pa)
				{
					return this.hatPairValidator(pa);
				}

				internal bool <>m__2(ThingStuffPair a)
				{
					return !ApparelUtility.CanWearTogether(a.thing, this.hatPair.thing, this.$this.body);
				}

				internal bool <>m__3(ThingStuffPair pa)
				{
					return !ApparelUtility.CanWearTogether(pa.thing, this.hatPair.thing, this.$this.body);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateWorkingPossibleApparelSetFor>c__AnonStorey0
		{
			internal List<ThingDef> reqApparel;

			public <GenerateWorkingPossibleApparelSetFor>c__AnonStorey0()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateWorkingPossibleApparelSetFor>c__AnonStorey1
		{
			internal int i;

			internal PawnApparelGenerator.<GenerateWorkingPossibleApparelSetFor>c__AnonStorey0 <>f__ref$0;

			public <GenerateWorkingPossibleApparelSetFor>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ThingStuffPair pa)
			{
				return pa.thing == this.<>f__ref$0.reqApparel[this.i];
			}
		}
	}
}

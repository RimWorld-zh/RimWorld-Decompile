using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public static class PawnApparelGenerator
	{
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
					ThingStuffPair thingStuffPair = this.aps[i];
					if (thingStuffPair.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						int num = 0;
						while (true)
						{
							int num2 = num;
							ThingStuffPair thingStuffPair2 = this.aps[i];
							if (num2 < thingStuffPair2.thing.apparel.layers.Count)
							{
								ThingStuffPair thingStuffPair3 = this.aps[i];
								ApparelLayer apparelLayer = thingStuffPair3.thing.apparel.layers[num];
								if (apparelLayer == ApparelLayer.OnSkin)
								{
									flag2 = true;
								}
								if (apparelLayer == ApparelLayer.Shell || apparelLayer == ApparelLayer.Middle)
								{
									flag = true;
								}
								num++;
								continue;
							}
							break;
						}
					}
				}
				return flag && !flag2;
			}

			public bool Covers(BodyPartGroupDef bp)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					ThingStuffPair thingStuffPair = this.aps[i];
					if (thingStuffPair.thing.apparel.bodyPartGroups.Contains(bp))
					{
						return true;
					}
				}
				return false;
			}

			public bool IsNaked(Gender gender)
			{
				switch (gender)
				{
				case Gender.Male:
					return !this.Covers(BodyPartGroupDefOf.Legs);
				case Gender.Female:
					return !this.Covers(BodyPartGroupDefOf.Legs) || !this.Covers(BodyPartGroupDefOf.Torso);
				case Gender.None:
					return false;
				default:
					return false;
				}
			}

			public bool SatisfiesNeededWarmth(NeededWarmth warmth, bool mustBeSafe = false, float mapTemperature = 21f)
			{
				if (warmth == NeededWarmth.Any)
				{
					return true;
				}
				if (mustBeSafe && !GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(mapTemperature))
				{
					return false;
				}
				switch (warmth)
				{
				case NeededWarmth.Cool:
				{
					float num2 = this.aps.Sum((ThingStuffPair a) => a.InsulationHeat);
					return num2 >= -2.0;
				}
				case NeededWarmth.Warm:
				{
					float num = this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
					return num <= -52.0;
				}
				default:
					throw new NotImplementedException();
				}
			}

			public void AddFreeWarmthAsNeeded(NeededWarmth warmth)
			{
				if (warmth != 0 && warmth != NeededWarmth.Cool)
				{
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine();
						PawnApparelGenerator.debugSb.AppendLine("Trying to give free warm layer.");
					}
					if (!this.SatisfiesNeededWarmth(warmth, false, 21f))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine("Checking to give free torso-cover at max price " + PawnApparelGenerator.freeWarmParkaMaxPrice);
						}
						Predicate<ThingStuffPair> parkaPairValidator = delegate(ThingStuffPair pa)
						{
							if (pa.Price > PawnApparelGenerator.freeWarmParkaMaxPrice)
							{
								return false;
							}
							if (pa.InsulationCold > -40.0)
							{
								return false;
							}
							return true;
						};
						ThingStuffPair parkaPair;
						if ((from pa in PawnApparelGenerator.allApparelPairs
						where parkaPairValidator(pa)
						select pa).TryRandomElementByWeight<ThingStuffPair>((Func<ThingStuffPair, float>)((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price)), out parkaPair))
						{
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.AppendLine("Giving free torso-cover: " + parkaPair + " insulation=" + parkaPair.InsulationCold);
								foreach (ThingStuffPair item in from a in this.aps
								where !ApparelUtility.CanWearTogether(a.thing, parkaPair.thing, this.body)
								select a)
								{
									PawnApparelGenerator.debugSb.AppendLine("    -replaces " + item.ToString() + " InsulationCold=" + item.InsulationCold);
								}
							}
							this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, parkaPair.thing, this.body));
							this.aps.Add(parkaPair);
						}
					}
					if (!this.SatisfiesNeededWarmth(warmth, false, 21f))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine("Checking to give free hat at max price " + PawnApparelGenerator.freeWarmHatMaxPrice);
						}
						Predicate<ThingStuffPair> hatPairValidator = delegate(ThingStuffPair pa)
						{
							if (pa.Price > PawnApparelGenerator.freeWarmHatMaxPrice)
							{
								return false;
							}
							if (pa.InsulationCold > -7.0)
							{
								return false;
							}
							if (!pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) && !pa.thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
							{
								return false;
							}
							return true;
						};
						ThingStuffPair hatPair;
						if ((from pa in PawnApparelGenerator.allApparelPairs
						where hatPairValidator(pa)
						select pa).TryRandomElementByWeight<ThingStuffPair>((Func<ThingStuffPair, float>)((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price)), out hatPair))
						{
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.AppendLine("Giving free hat: " + hatPair + " insulation=" + hatPair.InsulationCold);
								foreach (ThingStuffPair item2 in from a in this.aps
								where !ApparelUtility.CanWearTogether(a.thing, hatPair.thing, this.body)
								select a)
								{
									PawnApparelGenerator.debugSb.AppendLine("    -replaces " + item2.ToString() + " InsulationCold=" + item2.InsulationCold);
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

			public void GiveToPawn(Pawn pawn)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					ThingStuffPair thingStuffPair = this.aps[i];
					ThingDef thing = thingStuffPair.thing;
					ThingStuffPair thingStuffPair2 = this.aps[i];
					Apparel apparel = (Apparel)ThingMaker.MakeThing(thing, thingStuffPair2.stuff);
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
						if (j != k)
						{
							ThingStuffPair thingStuffPair3 = this.aps[j];
							ThingDef thing2 = thingStuffPair3.thing;
							ThingStuffPair thingStuffPair4 = this.aps[k];
							if (!ApparelUtility.CanWearTogether(thing2, thingStuffPair4.thing, pawn.RaceProps.body))
							{
								Log.Error(pawn + " generated with apparel that cannot be worn together: " + this.aps[j] + ", " + this.aps[k]);
								return;
							}
						}
					}
				}
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
		}

		private static List<ThingStuffPair> allApparelPairs;

		private static float freeWarmParkaMaxPrice;

		private static float freeWarmHatMaxPrice;

		private static PossibleApparelSet workingSet;

		private static List<ThingStuffPair> usableApparel;

		private static StringBuilder debugSb;

		static PawnApparelGenerator()
		{
			PawnApparelGenerator.allApparelPairs = new List<ThingStuffPair>();
			PawnApparelGenerator.workingSet = new PossibleApparelSet();
			PawnApparelGenerator.usableApparel = new List<ThingStuffPair>();
			PawnApparelGenerator.debugSb = null;
			PawnApparelGenerator.Reset();
		}

		public static void Reset()
		{
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((ThingDef td) => td.IsApparel);
			PawnApparelGenerator.freeWarmParkaMaxPrice = (float)(int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Parka, ThingDefOf.Cloth) * 1.2999999523162842);
			PawnApparelGenerator.freeWarmHatMaxPrice = (float)(int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Tuque, ThingDefOf.Cloth) * 1.2999999523162842);
		}

		public static void GenerateStartingApparelFor(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.RaceProps.ToolUser && pawn.RaceProps.IsFlesh)
			{
				if (pawn.Faction == null)
				{
					Log.Error("Cannot generate apparel for faction-less pawn " + pawn);
				}
				else
				{
					pawn.apparel.DestroyAll(DestroyMode.Vanish);
					float randomInRange = pawn.kindDef.apparelMoney.RandomInRange;
					float mapTemperature = default(float);
					NeededWarmth neededWarmth = PawnApparelGenerator.ApparelWarmthNeededNow(pawn, request, out mapTemperature);
					bool flag = Rand.Value < pawn.kindDef.apparelAllowHeadwearChance;
					PawnApparelGenerator.debugSb = null;
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb = new StringBuilder();
						PawnApparelGenerator.debugSb.AppendLine("Generating apparel for " + pawn);
						PawnApparelGenerator.debugSb.AppendLine("Money: " + randomInRange.ToString("F0"));
						PawnApparelGenerator.debugSb.AppendLine("Needed warmth: " + neededWarmth);
						PawnApparelGenerator.debugSb.AppendLine("Headwear allowed: " + flag);
					}
					if (randomInRange >= 0.0010000000474974513)
					{
						int num = 0;
						while (true)
						{
							PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, flag);
							if (DebugViewSettings.logApparelGeneration)
							{
								PawnApparelGenerator.debugSb.Append(num.ToString().PadRight(5) + "Trying: " + PawnApparelGenerator.workingSet.ToString());
							}
							if (num < 10 && Rand.Value < 0.85000002384185791)
							{
								float num2 = Rand.Range(0.45f, 0.8f);
								float totalPrice = PawnApparelGenerator.workingSet.TotalPrice;
								if (totalPrice < randomInRange * num2)
								{
									if (DebugViewSettings.logApparelGeneration)
									{
										PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Spent $" + totalPrice.ToString("F0") + ", < " + ((float)(num2 * 100.0)).ToString("F0") + "% of money.");
									}
									goto IL_036d;
								}
							}
							if (num < 20 && Rand.Value < 0.97000002861022949 && !PawnApparelGenerator.workingSet.Covers(BodyPartGroupDefOf.Torso))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Does not cover torso.");
								}
							}
							else if (num < 30 && Rand.Value < 0.800000011920929 && PawnApparelGenerator.workingSet.CoatButNoShirt())
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Coat but no shirt.");
								}
							}
							else
							{
								if (num < 50)
								{
									bool mustBeSafe = num < 17;
									if (!PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, mustBeSafe, mapTemperature))
									{
										if (DebugViewSettings.logApparelGeneration)
										{
											PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Wrong warmth.");
										}
										goto IL_036d;
									}
								}
								if (num >= 80)
									break;
								if (!PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
									break;
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Naked.");
								}
							}
							goto IL_036d;
							IL_036d:
							num++;
						}
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.Append(" -- Approved! Total price: $" + PawnApparelGenerator.workingSet.TotalPrice.ToString("F0") + ", TotalInsulationCold: " + PawnApparelGenerator.workingSet.TotalInsulationCold);
						}
					}
					if ((!pawn.kindDef.apparelIgnoreSeasons || request.ForceAddFreeWarmLayerIfNeeded) && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, false, 21f))
					{
						PawnApparelGenerator.workingSet.AddFreeWarmthAsNeeded(neededWarmth);
					}
					if (DebugViewSettings.logApparelGeneration)
					{
						Log.Message(PawnApparelGenerator.debugSb.ToString());
					}
					PawnApparelGenerator.workingSet.GiveToPawn(pawn);
					PawnApparelGenerator.workingSet.Reset(null, null);
				}
			}
		}

		private static void GenerateWorkingPossibleApparelSetFor(Pawn pawn, float money, bool headwearAllowed)
		{
			PawnApparelGenerator.workingSet.Reset(pawn.RaceProps.body, pawn.def);
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
					money -= pair.Price;
				}
			}
			int specialSeed = Rand.Int;
			while (!(Rand.Value < 0.10000000149011612))
			{
				Predicate<ThingStuffPair> predicate = delegate(ThingStuffPair pa)
				{
					if (pa.Price > money)
					{
						return false;
					}
					if (!headwearAllowed && PawnApparelGenerator.IsHeadwear(pa.thing))
					{
						return false;
					}
					if (pa.stuff != null && !pawn.Faction.def.CanUseStuffForApparel(pa.stuff))
					{
						return false;
					}
					if (PawnApparelGenerator.workingSet.PairOverlapsAnything(pa))
					{
						return false;
					}
					if (!pawn.kindDef.apparelTags.NullOrEmpty())
					{
						bool flag2 = false;
						int num = 0;
						while (num < pawn.kindDef.apparelTags.Count)
						{
							int num2 = 0;
							while (num2 < pa.thing.apparel.tags.Count)
							{
								if (!(pawn.kindDef.apparelTags[num] == pa.thing.apparel.tags[num2]))
								{
									num2++;
									continue;
								}
								flag2 = true;
								break;
							}
							if (!flag2)
							{
								num++;
								continue;
							}
							break;
						}
						if (!flag2)
						{
							return false;
						}
					}
					if (pa.thing.generateAllowChance < 1.0 && Rand.ValueSeeded(specialSeed ^ pa.thing.index ^ 64128343) > pa.thing.generateAllowChance)
					{
						return false;
					}
					return true;
				};
				for (int j = 0; j < PawnApparelGenerator.allApparelPairs.Count; j++)
				{
					if (predicate(PawnApparelGenerator.allApparelPairs[j]))
					{
						PawnApparelGenerator.usableApparel.Add(PawnApparelGenerator.allApparelPairs[j]);
					}
				}
				ThingStuffPair pair2 = default(ThingStuffPair);
				bool flag = ((IEnumerable<ThingStuffPair>)PawnApparelGenerator.usableApparel).TryRandomElementByWeight<ThingStuffPair>((Func<ThingStuffPair, float>)((ThingStuffPair pa) => pa.Commonality), out pair2);
				PawnApparelGenerator.usableApparel.Clear();
				if (!flag)
					break;
				PawnApparelGenerator.workingSet.Add(pair2);
				money -= pair2.Price;
			}
		}

		private static bool IsHeadwear(ThingDef td)
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
			if (tile == -1)
			{
				mapTemperature = 21f;
				return NeededWarmth.Any;
			}
			NeededWarmth neededWarmth = NeededWarmth.Any;
			Twelfth twelfth = GenLocalDate.Twelfth(tile);
			mapTemperature = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			for (int i = 0; i < 2; i++)
			{
				NeededWarmth neededWarmth2 = PawnApparelGenerator.CalculateNeededWarmth(pawn, tile, twelfth);
				if (neededWarmth2 != 0)
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
					return neededWarmth;
				}
				return NeededWarmth.Any;
			}
			return neededWarmth;
		}

		public static NeededWarmth CalculateNeededWarmth(Pawn pawn, int tile, Twelfth twelfth)
		{
			float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			if (num < pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 4.0)
			{
				return NeededWarmth.Warm;
			}
			if (num > pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) + 4.0)
			{
				return NeededWarmth.Cool;
			}
			return NeededWarmth.Any;
		}

		internal static void MakeTableApparelPairs()
		{
			DebugTables.MakeTablesDialog(from p in PawnApparelGenerator.allApparelPairs
			orderby p.thing.defName descending
			select p, new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName), new TableDataGetter<ThingStuffPair>("stuff", (ThingStuffPair p) => (p.stuff == null) ? string.Empty : p.stuff.defName), new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString()), new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => ((float)(p.Commonality * 100.0)).ToString("F4")), new TableDataGetter<ThingStuffPair>("def-commonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F4")), new TableDataGetter<ThingStuffPair>("insulationCold", (ThingStuffPair p) => (p.InsulationCold != 0.0) ? p.InsulationCold.ToString() : string.Empty));
		}

		public static void MakeTableApparelPairsByThing()
		{
			PawnApparelGenerator.MakeTablePairsByThing(PawnApparelGenerator.allApparelPairs);
		}

		internal static void LogHeadwearApparelPairs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Listing all entries in allApparelPairs of headwear");
			foreach (ThingStuffPair item in from pa in PawnApparelGenerator.allApparelPairs
			where PawnApparelGenerator.IsHeadwear(pa.thing)
			orderby pa.thing.defName
			select pa)
			{
				ThingStuffPair current = item;
				stringBuilder.AppendLine(current + "  - " + current.commonalityMultiplier);
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void MakeTablePairsByThing(List<ThingStuffPair> pairList)
		{
			DefMap<ThingDef, float> totalCommMult = new DefMap<ThingDef, float>();
			DefMap<ThingDef, float> totalComm = new DefMap<ThingDef, float>();
			DefMap<ThingDef, int> pairCount = new DefMap<ThingDef, int>();
			foreach (ThingStuffPair pair in pairList)
			{
				ThingStuffPair current = pair;
				ThingDef thing;
				DefMap<ThingDef, float> defMap;
				(defMap = totalCommMult)[thing = current.thing] = defMap[thing] + current.commonalityMultiplier;
				ThingDef thing2;
				(defMap = totalComm)[thing2 = current.thing] = defMap[thing2] + current.Commonality;
				DefMap<ThingDef, int> defMap2;
				ThingDef thing3;
				(defMap2 = pairCount)[thing3 = current.thing] = defMap2[thing3] + 1;
			}
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where pairList.Any((ThingStuffPair pa) => pa.thing == d)
			select d, new TableDataGetter<ThingDef>("thing", (ThingDef t) => t.defName), new TableDataGetter<ThingDef>("pair count", (ThingDef t) => pairCount[t].ToString()), new TableDataGetter<ThingDef>("total commonality multiplier ", (ThingDef t) => totalCommMult[t].ToString("F4")), new TableDataGetter<ThingDef>("total commonality", (ThingDef t) => totalComm[t].ToString("F4")), new TableDataGetter<ThingDef>("def-commonality", (ThingDef t) => t.generateCommonality.ToString("F4")));
		}
	}
}

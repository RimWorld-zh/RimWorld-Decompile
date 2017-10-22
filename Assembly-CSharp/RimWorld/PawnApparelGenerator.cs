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
			private const float StartingMinTemperature = 12f;

			private const float TargetMinTemperature = -40f;

			private List<ThingStuffPair> aps = new List<ThingStuffPair>();

			private HashSet<ApparelUtility.LayerGroupPair> lgps = new HashSet<ApparelUtility.LayerGroupPair>();

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
					return this.aps.Sum((Func<ThingStuffPair, float>)((ThingStuffPair pa) => pa.Price));
				}
			}

			public float TotalInsulationCold
			{
				get
				{
					return this.aps.Sum((Func<ThingStuffPair, float>)((ThingStuffPair a) => a.InsulationCold));
				}
			}

			public void Reset()
			{
				this.aps.Clear();
				this.lgps.Clear();
			}

			public void Add(ThingStuffPair pair)
			{
				this.aps.Add(pair);
				ApparelUtility.GenerateLayerGroupPairs(pair.thing, (Action<ApparelUtility.LayerGroupPair>)delegate(ApparelUtility.LayerGroupPair lgp)
				{
					this.lgps.Add(lgp);
				});
			}

			public bool PairOverlapsAnything(ThingStuffPair pair)
			{
				bool conflicts = false;
				ApparelUtility.GenerateLayerGroupPairs(pair.thing, (Action<ApparelUtility.LayerGroupPair>)delegate(ApparelUtility.LayerGroupPair lgp)
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
				{
					return !this.Covers(BodyPartGroupDefOf.Legs);
				}
				case Gender.Female:
				{
					return !this.Covers(BodyPartGroupDefOf.Legs) || !this.Covers(BodyPartGroupDefOf.Torso);
				}
				case Gender.None:
				{
					return false;
				}
				default:
				{
					return false;
				}
				}
			}

			public bool SatisfiesNeededWarmth(NeededWarmth warmth)
			{
				switch (warmth)
				{
				case NeededWarmth.Any:
				{
					return true;
				}
				case NeededWarmth.Cool:
				{
					return true;
				}
				case NeededWarmth.Warm:
				{
					float num = this.aps.Sum((Func<ThingStuffPair, float>)((ThingStuffPair a) => a.InsulationCold));
					return num <= -52.0;
				}
				default:
				{
					throw new NotImplementedException();
				}
				}
			}

			public void AddFreeWarmthAsNeeded(NeededWarmth warmth)
			{
				switch (warmth)
				{
				case NeededWarmth.Any:
					return;
				case NeededWarmth.Cool:
					return;
				default:
				{
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine();
						PawnApparelGenerator.debugSb.AppendLine("Trying to give free warm layer.");
					}
					if (!this.SatisfiesNeededWarmth(warmth))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine("Checking to give free torso-cover at max price " + PawnApparelGenerator.freeWarmParkaMaxPrice);
						}
						Predicate<ThingStuffPair> parkaPairValidator = (Predicate<ThingStuffPair>)delegate(ThingStuffPair pa)
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
								where !ApparelUtility.CanWearTogether(a.thing, parkaPair.thing)
								select a)
								{
									PawnApparelGenerator.debugSb.AppendLine("    -replaces " + item.ToString() + " InsulationCold=" + item.InsulationCold);
								}
							}
							this.aps.RemoveAll((Predicate<ThingStuffPair>)((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, parkaPair.thing)));
							this.aps.Add(parkaPair);
						}
					}
					if (!this.SatisfiesNeededWarmth(warmth))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.AppendLine("Checking to give free hat at max price " + PawnApparelGenerator.freeWarmHatMaxPrice);
						}
						Predicate<ThingStuffPair> hatPairValidator = (Predicate<ThingStuffPair>)delegate(ThingStuffPair pa)
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
								where !ApparelUtility.CanWearTogether(a.thing, hatPair.thing)
								select a)
								{
									PawnApparelGenerator.debugSb.AppendLine("    -replaces " + item2.ToString() + " InsulationCold=" + item2.InsulationCold);
								}
							}
							this.aps.RemoveAll((Predicate<ThingStuffPair>)((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, hatPair.thing)));
							this.aps.Add(hatPair);
						}
					}
					if (DebugViewSettings.logApparelGeneration)
					{
						PawnApparelGenerator.debugSb.AppendLine("New TotalInsulationCold: " + this.TotalInsulationCold);
					}
					break;
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
				List<Apparel> wornApparel = pawn.apparel.WornApparel;
				if (wornApparel.Count > 4)
				{
					for (int j = 0; j < wornApparel.Count; j++)
					{
						for (int k = 0; k < wornApparel.Count; k++)
						{
							if (j != k && !ApparelUtility.CanWearTogether(wornApparel[j].def, wornApparel[k].def))
							{
								Log.Error(pawn + " generated with apparel that cannot be worn together: " + wornApparel[j] + ", " + wornApparel[k]);
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
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((Predicate<ThingDef>)((ThingDef td) => td.IsApparel));
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
					NeededWarmth neededWarmth = PawnApparelGenerator.ApparelWarmthNeededNow(pawn, request);
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
									goto IL_0354;
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
							else if (num < 50 && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Wrong warmth.");
								}
							}
							else
							{
								if (num >= 80)
									break;
								if (!PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
									break;
								if (DebugViewSettings.logApparelGeneration)
								{
									PawnApparelGenerator.debugSb.AppendLine(" -- Failed: Naked.");
								}
							}
							goto IL_0354;
							IL_0354:
							num++;
						}
						if (DebugViewSettings.logApparelGeneration)
						{
							PawnApparelGenerator.debugSb.Append(" -- Approved! Total price: $" + PawnApparelGenerator.workingSet.TotalPrice.ToString("F0") + ", TotalInsulationCold: " + PawnApparelGenerator.workingSet.TotalInsulationCold);
						}
					}
					if ((!pawn.kindDef.apparelIgnoreSeasons || request.ForceAddFreeWarmLayerIfNeeded) && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth))
					{
						PawnApparelGenerator.workingSet.AddFreeWarmthAsNeeded(neededWarmth);
					}
					if (DebugViewSettings.logApparelGeneration)
					{
						Log.Message(PawnApparelGenerator.debugSb.ToString());
					}
					PawnApparelGenerator.workingSet.GiveToPawn(pawn);
					PawnApparelGenerator.workingSet.Reset();
				}
			}
		}

		private static void GenerateWorkingPossibleApparelSetFor(Pawn pawn, float money, bool headwearAllowed)
		{
			PawnApparelGenerator.workingSet.Reset();
			List<ThingDef> reqApparel = pawn.kindDef.apparelRequired;
			if (reqApparel != null)
			{
				int i;
				_003CGenerateWorkingPossibleApparelSetFor_003Ec__AnonStorey324 _003CGenerateWorkingPossibleApparelSetFor_003Ec__AnonStorey;
				for (i = 0; i < reqApparel.Count; i++)
				{
					ThingStuffPair pair = (from pa in PawnApparelGenerator.allApparelPairs
					where pa.thing == _003CGenerateWorkingPossibleApparelSetFor_003Ec__AnonStorey.reqApparel[i]
					select pa).RandomElementByWeight((Func<ThingStuffPair, float>)((ThingStuffPair pa) => pa.Commonality));
					PawnApparelGenerator.workingSet.Add(pair);
					money -= pair.Price;
				}
			}
			int specialSeed = Rand.Int;
			while (!(Rand.Value < 0.10000000149011612))
			{
				Predicate<ThingStuffPair> predicate = (Predicate<ThingStuffPair>)delegate(ThingStuffPair pa)
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

		private static NeededWarmth ApparelWarmthNeededNow(Pawn pawn, PawnGenerationRequest request)
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
				return NeededWarmth.Any;
			}
			NeededWarmth neededWarmth = NeededWarmth.Any;
			Twelfth twelfth = GenLocalDate.Twelfth(tile);
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
			select p, new TableDataGetter<ThingStuffPair>("thing", (Func<ThingStuffPair, string>)((ThingStuffPair p) => p.thing.defName)), new TableDataGetter<ThingStuffPair>("stuff", (Func<ThingStuffPair, string>)((ThingStuffPair p) => (p.stuff == null) ? string.Empty : p.stuff.defName)), new TableDataGetter<ThingStuffPair>("price", (Func<ThingStuffPair, string>)((ThingStuffPair p) => p.Price.ToString())), new TableDataGetter<ThingStuffPair>("commonality", (Func<ThingStuffPair, string>)((ThingStuffPair p) => ((float)(p.Commonality * 100.0)).ToString("F4"))), new TableDataGetter<ThingStuffPair>("def-commonality", (Func<ThingStuffPair, string>)((ThingStuffPair p) => p.thing.generateCommonality.ToString("F4"))), new TableDataGetter<ThingStuffPair>("insulationCold", (Func<ThingStuffPair, string>)((ThingStuffPair p) => (p.InsulationCold != 0.0) ? p.InsulationCold.ToString() : string.Empty)));
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
			List<ThingStuffPair>.Enumerator enumerator = pairList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ThingStuffPair current = enumerator.Current;
					DefMap<ThingDef, float> defMap;
					DefMap<ThingDef, float> obj = defMap = totalCommMult;
					ThingDef thing;
					ThingDef def = thing = current.thing;
					float num = defMap[thing];
					obj[def] = num + current.commonalityMultiplier;
					DefMap<ThingDef, float> defMap2;
					DefMap<ThingDef, float> obj2 = defMap2 = totalComm;
					ThingDef def2 = thing = current.thing;
					num = defMap2[thing];
					obj2[def2] = num + current.Commonality;
					DefMap<ThingDef, int> defMap3;
					DefMap<ThingDef, int> obj3 = defMap3 = pairCount;
					ThingDef def3 = thing = current.thing;
					int num2 = defMap3[thing];
					obj3[def3] = num2 + 1;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			DebugTables.MakeTablesDialog(from d in DefDatabase<ThingDef>.AllDefs
			where pairList.Any((Predicate<ThingStuffPair>)((ThingStuffPair pa) => pa.thing == d))
			select d, new TableDataGetter<ThingDef>("thing", (Func<ThingDef, string>)((ThingDef t) => t.defName)), new TableDataGetter<ThingDef>("pair count", (Func<ThingDef, string>)((ThingDef t) => pairCount[t].ToString())), new TableDataGetter<ThingDef>("total commonality multiplier ", (Func<ThingDef, string>)((ThingDef t) => totalCommMult[t].ToString("F4"))), new TableDataGetter<ThingDef>("total commonality", (Func<ThingDef, string>)((ThingDef t) => totalComm[t].ToString("F4"))), new TableDataGetter<ThingDef>("def-commonality", (Func<ThingDef, string>)((ThingDef t) => t.generateCommonality.ToString("F4"))));
		}
	}
}

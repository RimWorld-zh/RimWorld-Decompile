using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	public static class ArenaUtility
	{
		private const int liveSimultaneous = 15;

		public static bool ValidateArenaCapability()
		{
			bool result;
			if (Find.World.info.planetCoverage < 0.299f)
			{
				Log.Error("Planet coverage must be 30%+ to ensure a representative mix of biomes.", false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public static void BeginArenaFight(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback)
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Debug_Arena);
			mapParent.Tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, true, (int tile) => lhs.Concat(rhs).Any((PawnKindDef pawnkind) => Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, pawnkind.race)));
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
			IntVec3 spot;
			IntVec3 spot2;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(orGenerateMap, out spot, out spot2);
			List<Pawn> lhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, lhs, spot, Faction.OfAncients);
			List<Pawn> rhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, rhs, spot2, Faction.OfAncientsHostile);
			DebugArena component = mapParent.GetComponent<DebugArena>();
			component.lhs = lhs2;
			component.rhs = rhs2;
			component.callback = callback;
		}

		public static List<Pawn> SpawnPawnSet(Map map, List<PawnKindDef> kinds, IntVec3 spot, Faction faction)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < kinds.Count; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(kinds[i], faction);
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spot, map, 12, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
				list.Add(pawn);
			}
			LordMaker.MakeNewLord(faction, new LordJob_DefendPoint(map.Center), map, list);
			return list;
		}

		private static bool ArenaFightQueue(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, ArenaUtility.ArenaSetState state)
		{
			bool result2;
			if (!ArenaUtility.ValidateArenaCapability())
			{
				result2 = false;
			}
			else if (state.live < 15)
			{
				ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
				{
					state.live--;
					callback(result);
				});
				state.live++;
				result2 = true;
			}
			else
			{
				result2 = false;
			}
			return result2;
		}

		public static void BeginArenaFightSet(int count, List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, Action report)
		{
			if (ArenaUtility.ValidateArenaCapability())
			{
				int remaining = count;
				ArenaUtility.ArenaSetState state = new ArenaUtility.ArenaSetState();
				for (int i = 0; i < count; i++)
				{
					Current.Game.GetComponent<GameComponent_DebugTools>().AddPerFrameCallback(() => ArenaUtility.ArenaFightQueue(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
					{
						callback(result);
						remaining--;
						if (remaining % 10 == 0)
						{
							report();
						}
					}, state));
				}
			}
		}

		public static void PerformBattleRoyale(IEnumerable<PawnKindDef> kindsEnumerable)
		{
			if (ArenaUtility.ValidateArenaCapability())
			{
				List<PawnKindDef> kinds = kindsEnumerable.ToList<PawnKindDef>();
				Dictionary<PawnKindDef, float> ratings = new Dictionary<PawnKindDef, float>();
				foreach (PawnKindDef pawnKindDef in kinds)
				{
					ratings[pawnKindDef] = EloUtility.CalculateRating(pawnKindDef.combatPower, 1500f, 60f);
				}
				int currentFights = 0;
				int completeFights = 0;
				Current.Game.GetComponent<GameComponent_DebugTools>().AddPerFrameCallback(delegate
				{
					bool result2;
					if (currentFights >= 15)
					{
						result2 = false;
					}
					else
					{
						PawnKindDef lhsDef = kinds.RandomElement<PawnKindDef>();
						PawnKindDef rhsDef = kinds.RandomElement<PawnKindDef>();
						float num = EloUtility.CalculateExpectation(ratings[lhsDef], ratings[rhsDef]);
						float num2 = 1f - num;
						float num3 = num;
						float num4 = Mathf.Min(num2, num3);
						num2 /= num4;
						num3 /= num4;
						float num5 = Mathf.Max(num2, num3);
						if (num5 > 40f)
						{
							result2 = false;
						}
						else
						{
							float num6 = 40f / num5;
							float num7 = (float)Math.Exp((double)Rand.Range(0f, (float)Math.Log((double)num6)));
							num2 *= num7;
							num3 *= num7;
							List<PawnKindDef> lhs = Enumerable.Repeat<PawnKindDef>(lhsDef, GenMath.RoundRandom(num2)).ToList<PawnKindDef>();
							List<PawnKindDef> rhs = Enumerable.Repeat<PawnKindDef>(rhsDef, GenMath.RoundRandom(num3)).ToList<PawnKindDef>();
							currentFights++;
							ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
							{
								currentFights--;
								completeFights++;
								if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
								{
									float value = ratings[lhsDef];
									float value2 = ratings[rhsDef];
									float kfactor = 8f * Mathf.Pow(0.5f, Time.realtimeSinceStartup / 900f);
									EloUtility.Update(ref value, ref value2, 0.5f, (float)((result.winner != ArenaUtility.ArenaResult.Winner.Lhs) ? 0 : 1), kfactor);
									ratings[lhsDef] = value;
									ratings[rhsDef] = value2;
									Log.Message(string.Format("Scores after {0} trials:\n\n{1}", completeFights, (from v in ratings
									orderby v.Value
									select string.Format("  {0}: {1}->{2} (rating {2})", new object[]
									{
										v.Key.label,
										v.Key.combatPower,
										EloUtility.CalculateLinearScore(v.Value, 1500f, 60f).ToString("F0"),
										v.Value.ToString("F0")
									})).ToLineList("")), false);
								}
							});
							result2 = false;
						}
					}
					return result2;
				});
			}
		}

		public struct ArenaResult
		{
			public ArenaUtility.ArenaResult.Winner winner;

			public int tickDuration;

			public enum Winner
			{
				Other,
				Lhs,
				Rhs
			}
		}

		private class ArenaSetState
		{
			public int live = 0;

			public ArenaSetState()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <BeginArenaFight>c__AnonStorey0
		{
			internal List<PawnKindDef> lhs;

			internal List<PawnKindDef> rhs;

			public <BeginArenaFight>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int tile)
			{
				return this.lhs.Concat(this.rhs).Any((PawnKindDef pawnkind) => Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, pawnkind.race));
			}

			private sealed class <BeginArenaFight>c__AnonStorey1
			{
				internal int tile;

				internal ArenaUtility.<BeginArenaFight>c__AnonStorey0 <>f__ref$0;

				public <BeginArenaFight>c__AnonStorey1()
				{
				}

				internal bool <>m__0(PawnKindDef pawnkind)
				{
					return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(this.tile, pawnkind.race);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ArenaFightQueue>c__AnonStorey2
		{
			internal ArenaUtility.ArenaSetState state;

			internal Action<ArenaUtility.ArenaResult> callback;

			public <ArenaFightQueue>c__AnonStorey2()
			{
			}

			internal void <>m__0(ArenaUtility.ArenaResult result)
			{
				this.state.live--;
				this.callback(result);
			}
		}

		[CompilerGenerated]
		private sealed class <BeginArenaFightSet>c__AnonStorey3
		{
			internal List<PawnKindDef> lhs;

			internal List<PawnKindDef> rhs;

			internal ArenaUtility.ArenaSetState state;

			internal Action<ArenaUtility.ArenaResult> callback;

			internal int remaining;

			internal Action report;

			public <BeginArenaFightSet>c__AnonStorey3()
			{
			}

			internal bool <>m__0()
			{
				return ArenaUtility.ArenaFightQueue(this.lhs, this.rhs, delegate(ArenaUtility.ArenaResult result)
				{
					this.callback(result);
					this.remaining--;
					if (this.remaining % 10 == 0)
					{
						this.report();
					}
				}, this.state);
			}

			internal void <>m__1(ArenaUtility.ArenaResult result)
			{
				this.callback(result);
				this.remaining--;
				if (this.remaining % 10 == 0)
				{
					this.report();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PerformBattleRoyale>c__AnonStorey4
		{
			internal int currentFights;

			internal List<PawnKindDef> kinds;

			internal Dictionary<PawnKindDef, float> ratings;

			internal int completeFights;

			public <PerformBattleRoyale>c__AnonStorey4()
			{
			}

			internal bool <>m__0()
			{
				bool result2;
				if (this.currentFights >= 15)
				{
					result2 = false;
				}
				else
				{
					PawnKindDef lhsDef = this.kinds.RandomElement<PawnKindDef>();
					PawnKindDef rhsDef = this.kinds.RandomElement<PawnKindDef>();
					float num = EloUtility.CalculateExpectation(this.ratings[lhsDef], this.ratings[rhsDef]);
					float num2 = 1f - num;
					float num3 = num;
					float num4 = Mathf.Min(num2, num3);
					num2 /= num4;
					num3 /= num4;
					float num5 = Mathf.Max(num2, num3);
					if (num5 > 40f)
					{
						result2 = false;
					}
					else
					{
						float num6 = 40f / num5;
						float num7 = (float)Math.Exp((double)Rand.Range(0f, (float)Math.Log((double)num6)));
						num2 *= num7;
						num3 *= num7;
						List<PawnKindDef> lhs = Enumerable.Repeat<PawnKindDef>(lhsDef, GenMath.RoundRandom(num2)).ToList<PawnKindDef>();
						List<PawnKindDef> rhs = Enumerable.Repeat<PawnKindDef>(rhsDef, GenMath.RoundRandom(num3)).ToList<PawnKindDef>();
						this.currentFights++;
						ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
						{
							this.currentFights--;
							this.completeFights++;
							if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
							{
								float value = this.ratings[lhsDef];
								float value2 = this.ratings[rhsDef];
								float kfactor = 8f * Mathf.Pow(0.5f, Time.realtimeSinceStartup / 900f);
								EloUtility.Update(ref value, ref value2, 0.5f, (float)((result.winner != ArenaUtility.ArenaResult.Winner.Lhs) ? 0 : 1), kfactor);
								this.ratings[lhsDef] = value;
								this.ratings[rhsDef] = value2;
								Log.Message(string.Format("Scores after {0} trials:\n\n{1}", this.completeFights, (from v in this.ratings
								orderby v.Value
								select string.Format("  {0}: {1}->{2} (rating {2})", new object[]
								{
									v.Key.label,
									v.Key.combatPower,
									EloUtility.CalculateLinearScore(v.Value, 1500f, 60f).ToString("F0"),
									v.Value.ToString("F0")
								})).ToLineList("")), false);
							}
						});
						result2 = false;
					}
				}
				return result2;
			}

			private sealed class <PerformBattleRoyale>c__AnonStorey5
			{
				internal PawnKindDef lhsDef;

				internal PawnKindDef rhsDef;

				internal ArenaUtility.<PerformBattleRoyale>c__AnonStorey4 <>f__ref$4;

				private static Func<KeyValuePair<PawnKindDef, float>, float> <>f__am$cache0;

				private static Func<KeyValuePair<PawnKindDef, float>, string> <>f__am$cache1;

				public <PerformBattleRoyale>c__AnonStorey5()
				{
				}

				internal void <>m__0(ArenaUtility.ArenaResult result)
				{
					this.<>f__ref$4.currentFights = this.<>f__ref$4.currentFights - 1;
					this.<>f__ref$4.completeFights = this.<>f__ref$4.completeFights + 1;
					if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
					{
						float value = this.<>f__ref$4.ratings[this.lhsDef];
						float value2 = this.<>f__ref$4.ratings[this.rhsDef];
						float kfactor = 8f * Mathf.Pow(0.5f, Time.realtimeSinceStartup / 900f);
						EloUtility.Update(ref value, ref value2, 0.5f, (float)((result.winner != ArenaUtility.ArenaResult.Winner.Lhs) ? 0 : 1), kfactor);
						this.<>f__ref$4.ratings[this.lhsDef] = value;
						this.<>f__ref$4.ratings[this.rhsDef] = value2;
						Log.Message(string.Format("Scores after {0} trials:\n\n{1}", this.<>f__ref$4.completeFights, (from v in this.<>f__ref$4.ratings
						orderby v.Value
						select string.Format("  {0}: {1}->{2} (rating {2})", new object[]
						{
							v.Key.label,
							v.Key.combatPower,
							EloUtility.CalculateLinearScore(v.Value, 1500f, 60f).ToString("F0"),
							v.Value.ToString("F0")
						})).ToLineList("")), false);
					}
				}

				private static float <>m__1(KeyValuePair<PawnKindDef, float> v)
				{
					return v.Value;
				}

				private static string <>m__2(KeyValuePair<PawnKindDef, float> v)
				{
					return string.Format("  {0}: {1}->{2} (rating {2})", new object[]
					{
						v.Key.label,
						v.Key.combatPower,
						EloUtility.CalculateLinearScore(v.Value, 1500f, 60f).ToString("F0"),
						v.Value.ToString("F0")
					});
				}
			}
		}
	}
}

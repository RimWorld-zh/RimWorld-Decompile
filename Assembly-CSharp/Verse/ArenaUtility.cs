using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000F09 RID: 3849
	public static class ArenaUtility
	{
		// Token: 0x06005C5C RID: 23644 RVA: 0x002ED530 File Offset: 0x002EB930
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

		// Token: 0x06005C5D RID: 23645 RVA: 0x002ED574 File Offset: 0x002EB974
		public static void BeginArenaFight(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback)
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Debug_Arena);
			mapParent.Tile = TileFinder.RandomFactionBaseTileFor(Faction.OfPlayer, true, (int tile) => lhs.Concat(rhs).Any((PawnKindDef pawnkind) => Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, pawnkind.race)));
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

		// Token: 0x06005C5E RID: 23646 RVA: 0x002ED648 File Offset: 0x002EBA48
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

		// Token: 0x06005C5F RID: 23647 RVA: 0x002ED6C4 File Offset: 0x002EBAC4
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

		// Token: 0x06005C60 RID: 23648 RVA: 0x002ED740 File Offset: 0x002EBB40
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

		// Token: 0x06005C61 RID: 23649 RVA: 0x002ED7C4 File Offset: 0x002EBBC4
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

		// Token: 0x04003CE8 RID: 15592
		private const int liveSimultaneous = 15;

		// Token: 0x02000F0A RID: 3850
		public struct ArenaResult
		{
			// Token: 0x04003CE9 RID: 15593
			public ArenaUtility.ArenaResult.Winner winner;

			// Token: 0x04003CEA RID: 15594
			public int tickDuration;

			// Token: 0x02000F0B RID: 3851
			public enum Winner
			{
				// Token: 0x04003CEC RID: 15596
				Other,
				// Token: 0x04003CED RID: 15597
				Lhs,
				// Token: 0x04003CEE RID: 15598
				Rhs
			}
		}

		// Token: 0x02000F0C RID: 3852
		private class ArenaSetState
		{
			// Token: 0x04003CEF RID: 15599
			public int live = 0;
		}
	}
}

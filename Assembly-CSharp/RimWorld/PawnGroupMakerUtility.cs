using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000492 RID: 1170
	[HasDebugOutput]
	public class PawnGroupMakerUtility
	{
		// Token: 0x04000C7F RID: 3199
		private static readonly SimpleCurve PawnWeightFactorByMostExpensivePawnCostFractionCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.3f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			}
		};

		// Token: 0x060014B9 RID: 5305 RVA: 0x000B6594 File Offset: 0x000B4994
		public static IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			if (parms.groupKind == null)
			{
				Log.Error("Tried to generate pawns with null pawn group kind def. parms=" + parms, false);
				yield break;
			}
			if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no any PawnGroupMakers."
				}), false);
				yield break;
			}
			IEnumerable<PawnGroupMaker> usableGroupMakers = from gm in parms.faction.def.pawnGroupMakers
			where gm.kindDef == parms.groupKind && gm.CanGenerateFrom(parms)
			select gm;
			PawnGroupMaker chosenGroupMaker;
			if (!usableGroupMakers.TryRandomElementByWeight((PawnGroupMaker gm) => gm.commonality, out chosenGroupMaker))
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no usable PawnGroupMakers for parms ",
					parms
				}), false);
				yield break;
			}
			foreach (Pawn p in chosenGroupMaker.GeneratePawns(parms, warnOnZeroResults))
			{
				yield return p;
			}
			yield break;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x000B65C8 File Offset: 0x000B49C8
		public static IEnumerable<PawnGenOption> ChoosePawnGenOptionsByPoints(float pointsTotal, List<PawnGenOption> options, PawnGroupMakerParms groupParms)
		{
			float num = PawnGroupMakerUtility.MaxPawnCost(groupParms.faction, pointsTotal, groupParms.raidStrategy, groupParms.groupKind);
			List<PawnGenOption> list = new List<PawnGenOption>();
			List<PawnGenOption> list2 = new List<PawnGenOption>();
			float num2 = pointsTotal;
			bool flag = false;
			float highestCost = -1f;
			for (;;)
			{
				list.Clear();
				for (int i = 0; i < options.Count; i++)
				{
					PawnGenOption pawnGenOption = options[i];
					if (pawnGenOption.Cost <= num2)
					{
						if (pawnGenOption.Cost <= num)
						{
							if (!groupParms.generateFightersOnly || pawnGenOption.kind.isFighter)
							{
								if (groupParms.raidStrategy == null || groupParms.raidStrategy.Worker.CanUsePawnGenOption(pawnGenOption, list2))
								{
									if (!groupParms.dontUseSingleUseRocketLaunchers || pawnGenOption.kind.weaponTags == null || !pawnGenOption.kind.weaponTags.Contains("GunHeavy"))
									{
										if (!flag || !pawnGenOption.kind.factionLeader)
										{
											if (pawnGenOption.Cost > highestCost)
											{
												highestCost = pawnGenOption.Cost;
											}
											list.Add(pawnGenOption);
										}
									}
								}
							}
						}
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				Func<PawnGenOption, float> weightSelector = delegate(PawnGenOption gr)
				{
					float selectionWeight = gr.selectionWeight;
					return selectionWeight * PawnGroupMakerUtility.PawnWeightFactorByMostExpensivePawnCostFractionCurve.Evaluate(gr.Cost / highestCost);
				};
				PawnGenOption pawnGenOption2 = list.RandomElementByWeight(weightSelector);
				list2.Add(pawnGenOption2);
				num2 -= pawnGenOption2.Cost;
				if (pawnGenOption2.kind.factionLeader)
				{
					flag = true;
				}
			}
			if (list2.Count == 1 && num2 > pointsTotal / 2f)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Used only ",
					pointsTotal - num2,
					" / ",
					pointsTotal,
					" points generating for ",
					groupParms.faction
				}), false);
			}
			return list2;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000B67F0 File Offset: 0x000B4BF0
		public static float MaxPawnCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy, PawnGroupKindDef groupKind)
		{
			float num = faction.def.maxPawnCostPerTotalPointsCurve.Evaluate(totalPoints);
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, faction.def.MinPointsToGeneratePawnGroup(groupKind) * 1.2f);
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction, groupKind) * 1.2f);
			}
			return num;
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x000B6868 File Offset: 0x000B4C68
		public static bool CanGenerateAnyNormalGroup(Faction faction, float points)
		{
			bool result;
			if (faction.def.pawnGroupMakers == null)
			{
				result = false;
			}
			else
			{
				PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
				pawnGroupMakerParms.faction = faction;
				pawnGroupMakerParms.points = points;
				for (int i = 0; i < faction.def.pawnGroupMakers.Count; i++)
				{
					PawnGroupMaker pawnGroupMaker = faction.def.pawnGroupMakers[i];
					if (pawnGroupMaker.kindDef == PawnGroupKindDefOf.Combat)
					{
						if (pawnGroupMaker.CanGenerateFrom(pawnGroupMakerParms))
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x000B6908 File Offset: 0x000B4D08
		[DebugOutput]
		public static void PawnGroupsMade()
		{
			Dialog_DebugOptionListLister.ShowSimpleDebugMenu<Faction>(from fac in Find.FactionManager.AllFactions
			where !fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select fac, (Faction fac) => fac.Name + " (" + fac.def.defName + ")", delegate(Faction fac)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(string.Concat(new object[]
				{
					"FACTION: ",
					fac.Name,
					" (",
					fac.def.defName,
					") min=",
					fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat)
				}));
				Action<float> action = delegate(float points)
				{
					if (points >= fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))
					{
						PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
						pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
						pawnGroupMakerParms.tile = Find.CurrentMap.Tile;
						pawnGroupMakerParms.points = points;
						pawnGroupMakerParms.faction = fac;
						sb.AppendLine(string.Concat(new object[]
						{
							"Group with ",
							pawnGroupMakerParms.points,
							" points (max option cost: ",
							PawnGroupMakerUtility.MaxPawnCost(fac, points, RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat),
							")"
						}));
						float num2 = 0f;
						foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, false).OrderBy((Pawn pa) => pa.kindDef.combatPower))
						{
							string text;
							if (pawn.equipment.Primary != null)
							{
								text = pawn.equipment.Primary.Label;
							}
							else
							{
								text = "no-equipment";
							}
							Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
							string text2;
							if (apparel != null)
							{
								text2 = apparel.LabelCap;
							}
							else
							{
								text2 = "shirtless";
							}
							sb.AppendLine(string.Concat(new string[]
							{
								"  ",
								pawn.kindDef.combatPower.ToString("F0").PadRight(6),
								pawn.kindDef.defName,
								", ",
								text,
								", ",
								text2
							}));
							num2 += pawn.kindDef.combatPower;
						}
						sb.AppendLine("         totalCost " + num2);
						sb.AppendLine();
					}
				};
				foreach (float num in Dialog_DebugActionsMenu.PointsOptions(false))
				{
					float obj = num;
					action(obj);
				}
				Log.Message(sb.ToString(), false);
			});
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x000B6984 File Offset: 0x000B4D84
		public static bool TryGetRandomFactionForCombatPawnGroup(float points, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			List<Faction> source = Find.FactionManager.AllFactions.Where(delegate(Faction f)
			{
				if ((allowHidden || !f.def.hidden) && (allowDefeated || !f.defeated) && (allowNonHumanlike || f.def.humanlikeFaction) && (allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null)
				{
					if (f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat) && (validator == null || validator(f)))
					{
						return points >= f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat);
					}
				}
				return false;
			}).ToList<Faction>();
			return source.TryRandomElementByWeight((Faction f) => f.def.RaidCommonalityFromPoints(points), out faction);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnGroupMakerUtility
	{
		private const string MeleeWeaponTag = "Melee";

		private const float CostWeightDenominator = 100f;

		private static readonly SimpleCurve MaxPawnCostPerRaidPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 25f),
				true
			},
			{
				new CurvePoint(100f, 40f),
				true
			},
			{
				new CurvePoint(300f, 50f),
				true
			},
			{
				new CurvePoint(700f, 100f),
				true
			},
			{
				new CurvePoint(1000f, 150f),
				true
			},
			{
				new CurvePoint(1500f, 200f),
				true
			},
			{
				new CurvePoint(2000f, 400f),
				true
			},
			{
				new CurvePoint(3000f, 800f),
				true
			},
			{
				new CurvePoint(100000f, 50000f),
				true
			}
		};

		private static readonly SimpleCurve DesireToSuppressCountPerRaidPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(600f, 0f),
				true
			},
			{
				new CurvePoint(1000f, 0.5f),
				true
			},
			{
				new CurvePoint(2000f, 1f),
				true
			},
			{
				new CurvePoint(4000f, 2f),
				true
			}
		};

		public static IEnumerable<Pawn> GeneratePawns(PawnGroupKindDef groupKind, PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			if (groupKind == null)
			{
				Log.Error("Tried to generate pawns with null pawn group kind def. parms=" + parms);
			}
			else if (parms.faction.def.pawnGroupMakers == null)
			{
				Log.Error("Faction " + parms.faction + " of def " + parms.faction.def + " has no any PawnGroupMakers.");
			}
			else
			{
				IEnumerable<PawnGroupMaker> usableGroupMakers = from gm in parms.faction.def.pawnGroupMakers
				where gm.kindDef == ((_003CGeneratePawns_003Ec__IteratorC9)/*Error near IL_00c6: stateMachine*/).groupKind && gm.CanGenerateFrom(((_003CGeneratePawns_003Ec__IteratorC9)/*Error near IL_00c6: stateMachine*/).parms)
				select gm;
				PawnGroupMaker chosenGroupMaker;
				if (!usableGroupMakers.TryRandomElementByWeight<PawnGroupMaker>((Func<PawnGroupMaker, float>)((PawnGroupMaker gm) => gm.commonality), out chosenGroupMaker))
				{
					Log.Error("Faction " + parms.faction + " of def " + parms.faction.def + " has no usable PawnGroupMakers for parms " + parms);
				}
				else
				{
					foreach (Pawn item in chosenGroupMaker.GeneratePawns(parms, warnOnZeroResults))
					{
						yield return item;
					}
				}
			}
		}

		public static IEnumerable<PawnGenOption> ChoosePawnGenOptionsByPoints(float points, List<PawnGenOption> options, PawnGroupMakerParms parms)
		{
			float num = PawnGroupMakerUtility.MaxAllowedPawnGenOptionCost(parms.faction, points, parms.raidStrategy);
			List<PawnGenOption> list = new List<PawnGenOption>();
			List<PawnGenOption> list2 = new List<PawnGenOption>();
			float num2 = points;
			bool flag = false;
			while (true)
			{
				list.Clear();
				for (int i = 0; i < options.Count; i++)
				{
					PawnGenOption pawnGenOption = options[i];
					if (!(pawnGenOption.Cost > num2) && !(pawnGenOption.Cost > num) && (!parms.generateFightersOnly || pawnGenOption.kind.isFighter) && (parms.raidStrategy == null || parms.raidStrategy.Worker.CanUsePawnGenOption(pawnGenOption, list2)) && (!flag || !pawnGenOption.kind.factionLeader))
					{
						list.Add(pawnGenOption);
					}
				}
				if (list.Count != 0)
				{
					float desireToSuppressCount = PawnGroupMakerUtility.DesireToSuppressCountPerRaidPointsCurve.Evaluate(points);
					Func<PawnGenOption, float> weightSelector = (Func<PawnGenOption, float>)delegate(PawnGenOption gr)
					{
						float num3 = (float)gr.selectionWeight;
						if (desireToSuppressCount > 0.0)
						{
							float b = (float)(num3 * (gr.Cost / 100.0));
							num3 = Mathf.Lerp(num3, b, desireToSuppressCount);
						}
						return num3;
					};
					PawnGenOption pawnGenOption2 = list.RandomElementByWeight(weightSelector);
					list2.Add(pawnGenOption2);
					num2 -= pawnGenOption2.Cost;
					if (pawnGenOption2.kind.factionLeader)
					{
						flag = true;
					}
					continue;
				}
				break;
			}
			if (list2.Count == 1 && num2 > points / 2.0)
			{
				Log.Warning("Used only " + (points - num2) + " / " + points + " points generating for " + parms.faction);
			}
			return list2;
		}

		private static float MaxAllowedPawnGenOptionCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy)
		{
			float num = PawnGroupMakerUtility.MaxPawnCostPerRaidPointsCurve.Evaluate(totalPoints);
			num *= faction.def.maxPawnOptionCostFactor;
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, (float)(faction.def.MinPointsToGenerateNormalPawnGroup() * 1.2000000476837158));
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, (float)(raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction) * 1.2000000476837158));
			}
			return num;
		}

		public static void LogPawnGroupsMade()
		{
			using (IEnumerator<Faction> enumerator = Find.FactionManager.AllFactions.GetEnumerator())
			{
				Faction fac;
				while (enumerator.MoveNext())
				{
					fac = enumerator.Current;
					if (!fac.def.pawnGroupMakers.NullOrEmpty())
					{
						StringBuilder sb = new StringBuilder();
						sb.AppendLine("FACTION: " + fac.Name + " (" + fac.def.defName + ") min=" + fac.def.MinPointsToGenerateNormalPawnGroup());
						Action<float> action = (Action<float>)delegate(float points)
						{
							if (!(points < fac.def.MinPointsToGenerateNormalPawnGroup()))
							{
								PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
								pawnGroupMakerParms.tile = Find.VisibleMap.Tile;
								pawnGroupMakerParms.points = points;
								pawnGroupMakerParms.faction = fac;
								sb.AppendLine("Group with " + pawnGroupMakerParms.points + " points (max option cost: " + PawnGroupMakerUtility.MaxAllowedPawnGenOptionCost(fac, points, RaidStrategyDefOf.ImmediateAttack) + ")");
								float num = 0f;
								foreach (Pawn item in from pa in PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Normal, pawnGroupMakerParms, false)
								orderby pa.kindDef.combatPower
								select pa)
								{
									string text = (item.equipment.Primary == null) ? "no-equipment" : item.equipment.Primary.Label;
									Apparel apparel = item.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
									string text2 = (apparel == null) ? "shirtless" : apparel.LabelCap;
									sb.AppendLine("  " + item.kindDef.combatPower.ToString("F0").PadRight(6) + item.kindDef.defName + ", " + text + ", " + text2);
									num += item.kindDef.combatPower;
								}
								sb.AppendLine("         totalCost " + num);
								sb.AppendLine();
							}
						};
						foreach (float item2 in Dialog_DebugActionsMenu.PointsOptions())
						{
							float obj = item2;
							action(obj);
						}
						Log.Message(sb.ToString());
					}
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnGroupMakerUtility
	{
		private const string MeleeWeaponTag = "Melee";

		[DebuggerHidden]
		public static IEnumerable<Pawn> GeneratePawns(PawnGroupKindDef groupKind, PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			PawnGroupMakerUtility.<GeneratePawns>c__IteratorC8 <GeneratePawns>c__IteratorC = new PawnGroupMakerUtility.<GeneratePawns>c__IteratorC8();
			<GeneratePawns>c__IteratorC.groupKind = groupKind;
			<GeneratePawns>c__IteratorC.parms = parms;
			<GeneratePawns>c__IteratorC.warnOnZeroResults = warnOnZeroResults;
			<GeneratePawns>c__IteratorC.<$>groupKind = groupKind;
			<GeneratePawns>c__IteratorC.<$>parms = parms;
			<GeneratePawns>c__IteratorC.<$>warnOnZeroResults = warnOnZeroResults;
			PawnGroupMakerUtility.<GeneratePawns>c__IteratorC8 expr_31 = <GeneratePawns>c__IteratorC;
			expr_31.$PC = -2;
			return expr_31;
		}

		public static IEnumerable<PawnGenOption> ChoosePawnGenOptionsByPoints(float points, List<PawnGenOption> options, PawnGroupMakerParms parms)
		{
			float num = points;
			List<PawnGenOption> list = new List<PawnGenOption>();
			List<PawnGenOption> list2 = new List<PawnGenOption>();
			bool flag = false;
			while (true)
			{
				list.Clear();
				for (int i = 0; i < options.Count; i++)
				{
					PawnGenOption pawnGenOption = options[i];
					if (pawnGenOption.Cost <= num)
					{
						if (pawnGenOption.Cost <= PawnGroupMakerUtility.MaxAllowedPawnGenOptionCost(parms.faction, points, parms.raidStrategy))
						{
							if (!parms.generateFightersOnly || pawnGenOption.kind.isFighter)
							{
								if (parms.generateMeleeOnly)
								{
									if (pawnGenOption.kind.weaponTags.Any((string tag) => tag != "Melee"))
									{
										goto IL_F8;
									}
								}
								if (parms.raidStrategy == null || parms.raidStrategy.Worker.CanUsePawnGenOption(pawnGenOption, list2))
								{
									list.Add(pawnGenOption);
								}
							}
						}
					}
					IL_F8:;
				}
				if (flag)
				{
					list.RemoveAll((PawnGenOption group) => group.kind.factionLeader);
				}
				if (list.Count == 0)
				{
					break;
				}
				float desireToSuppressCount = Mathf.InverseLerp(800f, 1600f, points);
				desireToSuppressCount = Mathf.Clamp(desireToSuppressCount, 0f, 0.5f);
				Func<PawnGenOption, float> weightSelector = delegate(PawnGenOption gr)
				{
					float num2 = (float)gr.selectionWeight;
					if (desireToSuppressCount > 0f)
					{
						float b = num2 * gr.Cost;
						num2 = Mathf.Lerp(num2, b, desireToSuppressCount);
					}
					return num2;
				};
				PawnGenOption pawnGenOption2 = list.RandomElementByWeight(weightSelector);
				if (pawnGenOption2.kind.factionLeader)
				{
					flag = true;
				}
				list2.Add(pawnGenOption2);
				num -= pawnGenOption2.Cost;
			}
			return list2;
		}

		private static float MaxAllowedPawnGenOptionCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy)
		{
			float num = Mathf.Max(totalPoints * 0.5f, 50f);
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, faction.def.MinPointsToGenerateNormalPawnGroup() * 1.2f);
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction) * 1.2f);
			}
			return num;
		}

		public static void LogPawnGroupsMade()
		{
			foreach (Faction fac in Find.FactionManager.AllFactions)
			{
				if (!fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendLine(string.Concat(new object[]
					{
						"======== FACTION: ",
						fac.Name,
						" (",
						fac.def.defName,
						") min=",
						fac.def.MinPointsToGenerateNormalPawnGroup(),
						" ======="
					}));
					Action<float> action = delegate(float points)
					{
						if (points < fac.def.MinPointsToGenerateNormalPawnGroup())
						{
							return;
						}
						PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
						pawnGroupMakerParms.tile = Find.VisibleMap.Tile;
						pawnGroupMakerParms.points = points;
						sb.AppendLine("Group with " + pawnGroupMakerParms.points + " points");
						float num = 0f;
						foreach (Pawn current in PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Normal, pawnGroupMakerParms, false))
						{
							string text;
							if (current.equipment.Primary != null)
							{
								text = current.equipment.Primary.Label;
							}
							else
							{
								text = "NoEquipment";
							}
							sb.AppendLine(string.Concat(new string[]
							{
								"  ",
								current.kindDef.combatPower.ToString("F0").PadRight(5),
								"- ",
								current.kindDef.defName,
								", ",
								text
							}));
							num += current.kindDef.combatPower;
						}
						sb.AppendLine("         totalCost " + num);
						sb.AppendLine();
					};
					action(35f);
					action(70f);
					action(135f);
					action(200f);
					action(300f);
					action(500f);
					action(800f);
					action(1200f);
					action(2000f);
					action(3000f);
					action(4000f);
					Log.Message(sb.ToString());
				}
			}
		}
	}
}
